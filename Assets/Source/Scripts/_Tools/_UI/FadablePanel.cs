using System.Collections;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadablePanel : MonoBehaviour, ICompletable, IViewable, IAutomaticFillComponents
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private FadablePanelData _data;

        private Coroutine _fadeInWork;
        private bool _isComplete;

        public bool IsComplete => _isComplete;
        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _canvasGroup.alpha = _data.StartAlpha;
            gameObject.SetActive(true);
            _isComplete = true;
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;
            float duration = _data.FadeUpDuration * ((_data.MaxAlpha - _canvasGroup.alpha) / (_data.MaxAlpha - _data.MinAlpha));
            StartFading(duration, _data.MaxAlpha);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;
            float duration = _data.FadeOutDuration * ((_data.MinAlpha - _canvasGroup.alpha) / (_data.MinAlpha - _data.MaxAlpha));
            StartFading(duration, _data.MinAlpha);
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

            _canvasGroup.alpha = targetAlpha;
            _isComplete = true;
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