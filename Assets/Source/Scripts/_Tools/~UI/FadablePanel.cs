using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadablePanel : MonoBehaviour, ICompletable, IViewable<CancellationTokenData, CancellationTokenData>, IAutomaticFillComponents
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private FadablePanelData _data;

        //private Coroutine _fadeInWork;
        private CancellationTokenSource _currentCTS;
        private bool _isComplete;

        public bool IsComplete => _isComplete;
        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _canvasGroup.alpha = _data.StartAlpha;
            gameObject.SetActive(true);
            _isComplete = true;
        }

        public void Show(CancellationTokenData tokenData)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            Utils.Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(tokenData.Token);

            if (_data.IsDeactivatable)
                gameObject.SetActive(true);

            StartFading(_data.FadeUpDuration, _data.MaxAlpha);
        }

        public void Hide(CancellationTokenData tokenData)
        {
            if (IsShown == false)
                return;

            IsShown = false;

            Utils.Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(tokenData.Token);

            StartFading(_data.FadeOutDuration, _data.MinAlpha);
        }

        //private void StartFading(float duration, float targetAlpha)
        //{
        //    _isComplete = false;

        //    if (_fadeInWork != null)
        //    {
        //        StopCoroutine(_fadeInWork);
        //    }

        //    _fadeInWork = StartCoroutine(FadeIn(duration, targetAlpha));
        //}

        private void StartFading(float duration, float targetAlpha)
        {
            _isComplete = false;

            FadeIn(duration, targetAlpha, _currentCTS.Token).Forget();
        }

        private async UniTask FadeIn(float duration, float targetAlpha, CancellationToken token)
        {
            float startAlpha = _canvasGroup.alpha;
            float timeInWork = 0f;
            float newAlpha;

            while (timeInWork < duration)
            {
                if (token.IsCancellationRequested)
                    break;

                timeInWork += Time.deltaTime;

                if (timeInWork > duration)
                {
                    timeInWork = duration;
                }

                newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeInWork / duration);
                _canvasGroup.alpha = newAlpha;

                await UniTask.Yield(cancellationToken: token);
            }

            if (_data.IsDeactivatable)
                if (_canvasGroup.alpha == 0)
                    gameObject.SetActive(false);

            _isComplete = true;
        }

        private void OnDisable()
        {
            Utils.Utils.DestroyCTS(ref _currentCTS);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadablePanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}