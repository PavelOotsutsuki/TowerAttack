using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    internal class DiscoverCardAI : DiscoverCard
    {
        [SerializeField] private Color _selectedFrameColor;
        [SerializeField] private Image _frameImage;
        [SerializeField] private float _selectedWaitDuration = 1f;

        private Color _defaultColor;

        public override void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler, float scaleFactor,
            float viewDuration, CancellationToken fightToken)
        {
            _defaultColor = _frameImage.color;

            base.Init(clickCallback, discoverClickHandler, scaleFactor, viewDuration, fightToken);
        }

        public override void Deactivate()
        {
            if (Token.IsCancellationRequested)
                return;

            if (IsActive == false)
                return;

            IsActive = false;

            gameObject.SetActive(false);
        }

        public override void Activate(DiscoverCardActivateData data)
        {
            if (Token.IsCancellationRequested)
                return;

            if (IsActive == true)
                return;

            IsActive = true;

            _frameImage.color = _defaultColor;

            DiscoverViewLogicData discoverViewLogicData = new DiscoverViewLogicData(data.CardHeight, data.CardWidth);

            ViewLogic.Show(discoverViewLogicData);

            gameObject.SetActive(true);
        }

        public override void StartClickActions()
        {
            ClickingImitation(Token).Forget();
        }

        private async UniTask ClickingImitation(CancellationToken token)
        {
            if (Token.IsCancellationRequested)
                return;

            try
            {
                _frameImage.color = _selectedFrameColor;

                await UniTask.WaitForSeconds(_selectedWaitDuration, cancellationToken: token);

                ClickCallback?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }
    }
}