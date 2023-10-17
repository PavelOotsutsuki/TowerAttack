using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView: MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _feature;

    public void Init(CardSO cardSO)
    {
        _icon.sprite = cardSO.Icon;
        _number.text = cardSO.Number.ToString();
        _name.text = cardSO.Name;
        _feature.text = cardSO.Feature;
    }
}
