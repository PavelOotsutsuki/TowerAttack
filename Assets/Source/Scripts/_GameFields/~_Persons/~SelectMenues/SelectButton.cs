using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using Tools;
using Tools.UI;
using Tools.Utils;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    public class SelectButton : FadableConfirmableButton, IWorkable
    {
        private IDeactivatable _clickCallback;
        //private Coroutine _workableCoroutine;
        private GameFieldInputRoot _inputRoot;
        private CancellationTokenSource _deactivatingCTS;
        private CancellationToken _fightToken;

        public void Init(IDeactivatable clickCallback, GameFieldInputRoot inputRoot, CancellationToken fightToken)
        {
            _clickCallback = clickCallback;
            IsActive = false;
            _inputRoot = inputRoot;
            _fightToken = fightToken;

            base.Init();

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            Utils.DestroyCTS(ref _deactivatingCTS);

            gameObject.SetActive(true);

            base.BaseActivate();

            IsActive = true;
        }

        protected override void OnEnterClick()
        {
            if (IsActive != true)
                return;

            _inputRoot.Pause();
            _clickCallback.Deactivate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            base.BaseDeactivate();

            IsActive = false;

            Utils.DestroyCTS(ref _deactivatingCTS);
            _deactivatingCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            Deactivating(_deactivatingCTS.Token).Forget();
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            try
            {
                await UniTask.WaitUntil(() => IsComplete, cancellationToken: token);

                gameObject.SetActive(false);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private void OnDisable()
        {
            Utils.DestroyCTS(ref _deactivatingCTS);
        }
    }
}