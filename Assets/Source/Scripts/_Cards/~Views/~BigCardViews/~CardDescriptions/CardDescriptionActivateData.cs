using System.Threading;
using Tools;
using UnityEngine;

namespace Cards.Views.BigCardViews.CardDescriptions
{
    public class CardDescriptionActivateData : CancellationTokenData
    {
        private readonly string _description;
        private readonly Color _activateColor;
        private readonly bool _isOutline;

        public CardDescriptionActivateData(string description, CancellationToken cardToken, Color activateColor, bool isOutline) : base(cardToken)
        {
            _description = description;
            _activateColor = activateColor;
            _isOutline = isOutline;
        }

        public CardDescriptionActivateData(string description, CancellationToken cardToken) : this(description, cardToken, Color.white, false)
        { }

        public string Description => _description;
        public Color ActivateColor => _activateColor;
        public bool IsOutline => _isOutline;
    }
}