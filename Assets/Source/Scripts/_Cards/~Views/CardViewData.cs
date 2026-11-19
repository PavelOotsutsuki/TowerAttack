using Tools;
using UnityEngine;

namespace Cards.Views
{
    public class CardViewData: IData//, IFeatureWatcher
    {
        private readonly Sprite _icon;
        private readonly int _number;
        private readonly string _name;
        private readonly string _description;

        private CardCapability _cardCapability;
        private string _feature;

        internal CardViewData(ICardViewConfig viewConfig, CardCapability cardCapability)
        {
            _icon = viewConfig.Icon;
            _number = viewConfig.Number;
            _name = viewConfig.Name;
            _description = viewConfig.Description;
            _feature = viewConfig.Feature;
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

        internal void SetCurseMode()
        {
            if ((_cardCapability & CardCapability.Curse) == CardCapability.Curse)
                return;

            _cardCapability |= CardCapability.Curse;
        }
    }
}