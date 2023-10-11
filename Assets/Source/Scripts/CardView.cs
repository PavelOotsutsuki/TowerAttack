using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView
{
    public CardView(CardSO cardSO, Image icon, TMP_Text number, TMP_Text name, TMP_Text feature, AudioSource audioSource)
    {
        icon.sprite = cardSO.Icon;
        number.text = cardSO.Number.ToString();
        name.text = cardSO.Name;
        feature.text = cardSO.Feature;
        audioSource.clip = cardSO.AwakeSound;
    }
}
