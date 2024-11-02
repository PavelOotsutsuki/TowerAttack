using System.Collections;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI.Fadings
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class FadablePanel : MonoBehaviour, ICompletable, IViewable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private FadablePanelData _data;

        private Coroutine _fadeInWork;
        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public void Init()
        {
            _canvasGroup.alpha = _data.StartAlpha;
            _isComplete = false;
        }

        public void Show()
        {
            gameObject.SetActive(true);

            StartFading(_data.FadeUpDuration, _data.MaxAlpha);
        }

        public void Hide()
        {
            StartFading(_data.FadeOutDuration, _data.MinAlpha);

            Deactivating().ToUniTask(); 
        }

        private void StartFading(float duration, float targetAlpha)
        {
            _isComplete = false;

            if (_fadeInWork != null)
            {
                StopCoroutine(_fadeInWork);
            }

            _fadeInWork = StartCoroutine(FadeIn(duration, targetAlpha));
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _isComplete);

            gameObject.SetActive(false);
        }

        private IEnumerator FadeIn(float duration, float targetAlpha)
        {
            float startAlpha = _canvasGroup.alpha;
            float timeInWork = 0f;
            float newAlpha;

            while (timeInWork < duration)
            {
                timeInWork += Time.deltaTime;

                if (timeInWork > duration)
                {
                    timeInWork = duration;
                }

                newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeInWork / duration);
                _canvasGroup.alpha = newAlpha;

                yield return null;
            }

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        public void DefineAllComponents()
        {
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}