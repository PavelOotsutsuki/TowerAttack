using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using Tools;
using Tools.UI;
using Tools.Utils;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuButton : FadableConfirmableButton
    {
        private IDeactivatable _clickCallback;
        //private Coroutine _workableCoroutine;
        private CancellationTokenSource _currentCTS;
        private CancellationToken _fightToken;

        private GameFieldInputRoot _inputRoot;

        public void Init(IDeactivatable clickCallback, GameFieldInputRoot inputRoot, CancellationToken fightToken)
        {
            _clickCallback = clickCallback;
            _inputRoot = inputRoot;
            _fightToken = fightToken;
            IsActive = false;

            base.Init();

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            gameObject.SetActive(true);

            base.BaseActivate2(new CancellationTokenData(_currentCTS.Token));

            //_inputRoot.SetInputType(InputType.LookCardMenu);
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

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            base.BaseDeactivate2(new CancellationTokenData(_currentCTS.Token));
            IsActive = false;

            Deactivating(_currentCTS.Token).Forget();
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
    }
}