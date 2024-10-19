using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    [Serializable]
    public class CardView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;

        public void FillData(CardViewConfig cardViewConfig)
        {
            _icon.sprite = cardViewConfig.Icon;
            _number.text = cardViewConfig.Number.ToString();
            _name.text = cardViewConfig.Name;
            _feature.text = cardViewConfig.Feature;
        }
    }
}