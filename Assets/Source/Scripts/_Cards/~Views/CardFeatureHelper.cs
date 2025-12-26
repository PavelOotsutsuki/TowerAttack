using System;
using Cards.Views.BigCardViews.CardDescriptions;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Cards.Views
{
    public class CardFeatureHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private readonly Color _descriptionActivateColor2 = new Color(236, 146, 146, 255);
        private readonly Color _descriptionActivateColor1 = new Color(255, 0, 0, 255);
        //private readonly Color _descriptionActivateColor = Color.red;
        private bool _isActive;

        private CardDescription _cardDescription;
        private Func<string> _textDescriptionGetter;

        public void Init(CardDescription cardDescription, Func<string> textDescriptionGetter)
        {
            _cardDescription = cardDescription;
            _textDescriptionGetter = textDescriptionGetter;
            _isActive = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Color descriptionActivateColor = Random.Range(0, 2) == 0? _descriptionActivateColor1 : _descriptionActivateColor2;
            CardDescriptionActivateData activateData = new CardDescriptionActivateData(_textDescriptionGetter.Invoke(),
                descriptionActivateColor);
            _cardDescription.Show(activateData);

            _isActive = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isActive == false)
                return;

            _cardDescription.Hide();
            _isActive = false;
        }
    }
}