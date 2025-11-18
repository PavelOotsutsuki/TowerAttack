using Tools;
using UnityEngine;

namespace Cards
{
    public class CardViewData: IData
    {
        private readonly Sprite _icon;
        private readonly int _number;
        private readonly string _name;
        private readonly string _description;
        private readonly CardCapability _cardCapability;

        private string _feature;

        internal CardViewData(CardViewConfig config, CardCapability cardCapability)
        {
            _icon = config.Icon;
            _number = config.Number;
            _name = config.Name;
            _description = config.Description;
            _feature = config.Feature;
            _cardCapability = cardCapability;
        }

        public Sprite Icon => _icon;
        public int Number => _number;
        public string Name => _name;
        public string Description => _description;
        public string Feature => _feature;
        public CardCapability CardCapability => _cardCapability;

        public void ChangeFeature(string feature)
        {
            _feature = feature;
        }
    }
}