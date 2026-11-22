using Tools;
using UnityEngine;

namespace Cards.Views
{
    public interface ICardViewConfig : IData
    {
        public int Number { get; }
        public Sprite Icon { get; }
        public string Description { get; }
        public string Feature { get; }
        public string Name { get; }
    }
}