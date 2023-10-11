using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehavior : IPointerEnterHandler
{
    private AudioSource _audioSource;

    public CardBehavior(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ok");
        _audioSource.Play();
    }
}
