using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDescription : MonoBehaviour
{
    private const float FadeOutDuration = 1.5f;
    private const float FadeUpDuration = 1.5f;
    private const float StartAlpha = 0f;
    private const string StartText = "";
    private const float MaxAlpha = 1f;
    private const float MinAlpha = 0f;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    private Coroutine _fadeInWork;

    public void Init()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, StartAlpha);
        _text.text = StartText;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, StartAlpha);
    }

    public void Show(string description)
    {
        IEnumerator startCoroutine = FadeIn(FadeUpDuration, MaxAlpha);

        StartCoroutine(ref _fadeInWork, startCoroutine);
        _text.text = description;
    }

    public void Hide()
    {
        IEnumerator startCoroutine = FadeIn(FadeOutDuration, MinAlpha);

        StartCoroutine(ref _fadeInWork, startCoroutine);
    }

    private void StartCoroutine(ref Coroutine container, IEnumerator startCoroutine)
    {
        TryStopCoroutine(container);

        container = StartCoroutine(startCoroutine);
    }

    private void TryStopCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator FadeIn(float duration, float targetAlpha)
    {
        float startAlpha = _image.color.a;
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
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, newAlpha);

            yield return true;
        }
    }

    [ContextMenu("DefineAllComponents")]
    private void DefineAllComponents()
    {
        DefineText();
        DefineImage();
    }

    [ContextMenu("DefineText")]
    private void DefineText()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    [ContextMenu("DefineImage")]
    private void DefineImage()
    {
        _image = GetComponent<Image>();
    }
}
