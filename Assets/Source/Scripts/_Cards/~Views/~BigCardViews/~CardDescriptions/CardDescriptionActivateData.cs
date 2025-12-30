using Tools;
using UnityEngine;

namespace Cards.Views.BigCardViews.CardDescriptions
{
    public class CardDescriptionActivateData : IData
    {
        private readonly string _description;
        private readonly Color _activateColor;
        private readonly bool _isOutline;

        public CardDescriptionActivateData(string description, Color activateColor, bool isOutline)
        {
            _description = description;
            _activateColor = activateColor;
            _isOutline = isOutline;
        }

        public CardDescriptionActivateData(string description) : this(description, Color.white, false)
        { }

        public string Description => _description;
        public Color ActivateColor => _activateColor;
        public bool IsOutline => _isOutline;
    }
}