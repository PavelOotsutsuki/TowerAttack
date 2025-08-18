using System;
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

        private string _feature;

        internal CardViewData(CardViewConfig config)
        {
            _icon = config.Icon;
            _number = config.Number;
            _name = config.Name;
            _description = config.Description;
            _feature = config.Feature;
        }

        public Sprite Icon => _icon;
        public int Number => _number;
        public string Name => _name;
        public string Description => _description;
        public string Feature => _feature;

        public void ChangeFeature(string feature)
        {
            _feature = feature;
        }
    }
}