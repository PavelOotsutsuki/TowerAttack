using System.Collections;
using TMPro;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class FadableLabel : MonoBehaviour, ICompletable
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private FadableLableData _data;

        private Coroutine _fadeInWork;
        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public void Init()
        {
            _text.text = _data.StartText;
            _canvasGroup.alpha = _data.StartAlpha;
            _isComplete = false;
        }

        public virtual void Show(string description)
        {
            Show();

            _text.text = description;
        }

        public virtual void Show()
        {
            StartFading(_data.FadeUpDuration, _data.MaxAlpha);
        }

        public virtual void Hide()
        {
            StartFading(_data.FadeOutDuration, _data.MinAlpha);
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

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineText();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineText))]
        private void DefineText()
        {
            AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}