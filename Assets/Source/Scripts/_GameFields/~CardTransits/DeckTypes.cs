using Tools;

namespace GameFields.CardTransits
{
    public class DeckTypes : IData
    {
        private readonly ViewType _viewType;
        private readonly TransitToType _toType;
        private readonly TowerTransitType _towerType;

        public DeckTypes()
        {
            _viewType = ViewType.Deck;
            _toType = TransitToType.Deck;
            _towerType = TowerTransitType.Deck;
        }

        public ViewType ViewType => _viewType;
        public TransitToType ToType => _toType;
        public TowerTransitType TowerType => _towerType;
    }
}
