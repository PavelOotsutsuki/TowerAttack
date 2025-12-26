using Tools;
using UnityEngine;

namespace Cards.Views.BigCardViews.CardDescriptions
{
    public class CardDescriptionActivateData : IData
    {
        private readonly string _description;
        private readonly Color _activateColor;

        public CardDescriptionActivateData(string description, Color activateColor)
        {
            _description = description;
            _activateColor = activateColor;
        }

        public CardDescriptionActivateData(string description) : this(description, Color.white)
        { }

        public string Description => _description;
        public Color ActivateColor => _activateColor;
    }
}