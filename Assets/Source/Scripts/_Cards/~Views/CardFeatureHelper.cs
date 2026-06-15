using System;
using System.Threading;
using Cards.Views.BigCardViews.CardDescriptions;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private CancellationToken _cardToken;

        public void Init(CardDescription cardDescription, Func<string> textDescriptionGetter, CancellationToken cardToken)
        {
            _cardDescription = cardDescription;
            _textDescriptionGetter = textDescriptionGetter;
            _cardToken = cardToken;
            _isActive = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CardDescriptionActivateData activateData = new CardDescriptionActivateData(_textDescriptionGetter.Invoke(),
                _cardToken, _descriptionActivateColor, true);
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