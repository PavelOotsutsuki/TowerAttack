using UnityEngine;

namespace Cards.Views
{
    public class CardViewConfigDB : ICardViewConfig
    {
        private readonly int _number;
        private readonly Sprite _icon;
        private readonly string _description;
        private readonly string _feature;
        private readonly string _name;
        public CardViewConfigDB(Sprite icon)
        {
            _number = 3;

            _icon = icon;
            // _icon = Sprite.Create(texture,
            // new Rect(0, 0, texture.width, texture.height),
            // new Vector2(0.5f, 0.5f),
            // 100f);

            _description = "Пусть все сгорит! Но пока только твои карты.";
            _feature = "Следующие 2 хода противник сжигает карты при взятии из колоды";
            _name = "Пиромант";
        }
        public int Number => _number;

        public Sprite Icon => _icon;

        public string Description => _description;

        public string Feature => _feature;

        public string Name => _name;
    }
}