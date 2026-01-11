using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.ConfirmableNumbersView
{
    public class ConfirmableNumbersViewRoot : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private ConfirmableNumbersPanel _panel;
        [SerializeField] private EnemyLabel _enemyLabel;
        [SerializeField] private PlayerLabel _playerLabel;

        private ConfirmableNumbers _enemyConfirmableNumbers;
        private ConfirmableNumbers _playerConfirmableNumbers;

        private Coroutine _deactivateCoroutine = null;
        private Coroutine _activateCoroutine = null;

        public bool? IsActive { get; private set; } = null;

        public void Init(ConfirmableNumbers enemyConfirmableNumbers, ConfirmableNumbers playerConfirmableNumbers)
        {
            _panel.Init();

            _enemyConfirmableNumbers = enemyConfirmableNumbers;
            _playerConfirmableNumbers = playerConfirmableNumbers;

            _enemyConfirmableNumbers.OnChanged += SetTextByEnemy;
            _playerConfirmableNumbers.OnChanged += SetTextByPlayer;

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            gameObject.SetActive(true);

            _activateCoroutine = StartCoroutine(Activating());
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            if (_activateCoroutine != null)
            {
                StopCoroutine(_activateCoroutine);
                _activateCoroutine = null;
            }

            _panel.Hide();

            _deactivateCoroutine = StartCoroutine(WaitUntilSetDeactivate());
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

            AppendList(stringBuilder, "ОСТАЛОСЬ", confirmableNumbers.FreeNumbers);

            stringBuilder.Append("\n--------------------\n");

            AppendList(stringBuilder, "ВЫБРАНО", confirmableNumbers.CheckedNumbers);

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
            _deactivateCoroutine = null;
            _activateCoroutine = null;
        }

        private IEnumerator Activating()
        {
            if (_deactivateCoroutine != null)
            {
                StopCoroutine(_deactivateCoroutine);
                _deactivateCoroutine = null;
            }
            else
            {
                yield return new WaitForSeconds(0.8f);
            }

            _panel.Show();
        }

        private IEnumerator WaitUntilSetDeactivate()
        {
            yield return new WaitUntil(() => _panel.IsComplete);

            gameObject.SetActive(false);
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