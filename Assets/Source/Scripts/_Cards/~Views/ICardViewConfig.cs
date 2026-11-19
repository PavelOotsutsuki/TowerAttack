using UnityEngine;

namespace Cards.Views
{
    internal interface ICardViewConfig
    {
        public int Number { get; }
        public Sprite Icon { get; }
        public string Description { get; }
        public string Feature { get; }
        public string Name { get; }
    }
}