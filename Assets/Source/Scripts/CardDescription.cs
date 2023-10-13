using System.Collections;
using TMPro;
using UnityEngine;

public class CardDescription : MonoBehaviour
{
    private const string StartText = "";

    [SerializeField] private TMP_Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _maxAlpha = 1f;
    [SerializeField] private float _minAlpha = 0f;
    [SerializeField] private float _fadeOutDuration = 1.5f;
    [SerializeField] private float _fadeUpDuration = 1.5f;
    [SerializeField] private float _startAlpha = 0f;

    private Coroutine _fadeInWork;

    public void Init()
    {
        _text.text = StartText;
        _canvasGroup.alpha = _startAlpha;
    }

    public void Show(string description)
    {
        StartFading(_fadeUpDuration, _maxAlpha);
        _text.text = description;
    }

    public void Hide()
    {
        StartFading(_fadeOutDuration, _minAlpha);
    }

    private void StartFading(float duration, float targetAlpha)
    {
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

            yield return true;
        }
    }

    [ContextMenu("DefineAllComponents")]
    private void DefineAllComponents()
    {
        DefineText();
        DefineCanvasGroup();
    }

    [ContextMenu("DefineText")]
    private void DefineText()
    {
        AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InChildren);
    }

    [ContextMenu("DefineCanvasGroup")]
    private void DefineCanvasGroup()
    {
        AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
    }
}
