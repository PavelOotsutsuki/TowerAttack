using System;
using Cards.Views.BigCardViews.CardDescriptions;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Cards.Views
{
    public class CardFeatureHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //private readonly Color _descriptionActivateColor = new Color32(236, 146, 146, 255); // Розовый 1
        //private readonly Color _descriptionActivateColor = new Color32(91, 82, 82, 255); 
        private readonly Color _descriptionActivateColor = new Color32(150, 138, 138, 255);
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
            CardDescriptionActivateData activateData = new CardDescriptionActivateData(_textDescriptionGetter.Invoke(),
                _descriptionActivateColor, true);
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