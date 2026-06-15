using System.Collections.Generic;
using System.Text;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using System.Reflection;
using Tools.Utils;

namespace GameFields.Persons.ConfirmableNumbersView
{
    public class ConfirmableNumbersViewRoot : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private ConfirmableNumbersPanel _panel;
        [SerializeField] private EnemyLabel _enemyLabel;
        [SerializeField] private PlayerLabel _playerLabel;

        private ConfirmableNumbers _enemyConfirmableNumbers;
        private ConfirmableNumbers _playerConfirmableNumbers;

        private CancellationTokenSource _activateCTS;
        private CancellationTokenSource _deactivateCTS;

        private CancellationToken _fightToken;
        //private Coroutine _deactivateCoroutine = null;
        //private Coroutine _activateCoroutine = null;

        public bool? IsActive { get; private set; } = null;

        public void Init(ConfirmableNumbers enemyConfirmableNumbers, ConfirmableNumbers playerConfirmableNumbers, CancellationToken fightToken)
        {
            _panel.Init();

            _enemyConfirmableNumbers = enemyConfirmableNumbers;
            _playerConfirmableNumbers = playerConfirmableNumbers;
            _fightToken = fightToken;

            SetTextByEnemy();
            SetTextByPlayer();

            _enemyConfirmableNumbers.OnChanged += SetTextByEnemy;
            _playerConfirmableNumbers.OnChanged += SetTextByPlayer;

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsActive == true)
                return;

            IsActive = true;

            gameObject.SetActive(true);

            Utils.DestroyCTS(ref _activateCTS);
            _activateCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            Activating(_activateCTS.Token).Forget();
        }

        public void Deactivate()
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsActive == false)
                return;

            IsActive = false;

            Utils.DestroyCTS(ref _activateCTS);
            Utils.DestroyCTS(ref _deactivateCTS);
            _deactivateCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _panel.Hide(new CancellationTokenData(_deactivateCTS.Token));

            WaitUntilSetDeactivate(_deactivateCTS.Token).Forget();
        }

        private void SetTextByEnemy()
        {
            SetText(_enemyConfirmableNumbers, _enemyLabel);
        }

        private void SetTextByPlayer()
        {
            SetText(_playerConfirmableNumbers, _playerLabel);
        }

        private void SetText(ConfirmableNumbers confirmableNumbers, Label label)
        {
            StringBuilder stringBuilder = new StringBuilder();

            AppendList(stringBuilder, "<color=#00A107>ОСТАЛОСЬ</color>", confirmableNumbers.FreeNumbers);

            stringBuilder.Append("\n--------------------\n");

            AppendList(stringBuilder, "<color=#FF0000>ВЫБРАНО</color>", confirmableNumbers.CheckedNumbers.OrderBy(n => n));

            label.SetText(stringBuilder.ToString());
        }

        private void AppendList(StringBuilder stringBuilder, string defaultText, IEnumerable<int> numbers)
        {
            stringBuilder.Append($"{defaultText}: ");

            bool isFirst = true;
            foreach (int number in numbers)
            {
                if (isFirst == false)
                    stringBuilder.Append(", ");

                stringBuilder.Append(number.ToString());
                isFirst = false;
            }
        }    

        private void OnDestroy()
        {
            _enemyConfirmableNumbers.OnChanged -= SetTextByEnemy;
            _playerConfirmableNumbers.OnChanged -= SetTextByPlayer;
        }

        private void OnDisable()
        {
            Utils.DestroyCTS(ref _activateCTS);
            Utils.DestroyCTS(ref _deactivateCTS);
        }

        private async UniTask Activating(CancellationToken activateToken)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                if (_deactivateCTS != null)
                {
                    Utils.DestroyCTS(ref _deactivateCTS);
                }
                else
                {
                    await UniTask.WaitForSeconds(0.8f, cancellationToken: activateToken);
                }

                _panel.Show(new CancellationTokenData(activateToken));
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask WaitUntilSetDeactivate(CancellationToken token)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                await UniTask.WaitUntil(() => _panel.IsComplete, cancellationToken: token);

                gameObject.SetActive(false);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ConfirmableNumbersViewRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerCardViewPanel(),
                DefineEnemyLabel(),
                DefinePlayerLabel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerCardViewPanel))]
        private ComponentAttachInfo DefineTowerCardViewPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _panel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEnemyLabel))]
        private ComponentAttachInfo DefineEnemyLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _enemyLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePlayerLabel))]
        private ComponentAttachInfo DefinePlayerLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _playerLabel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}