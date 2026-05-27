using Tools;

namespace GameFields.CardTransits
{
    public class DiscardPileTypes : IData
    {
        private readonly ViewType _viewType;
        private readonly TransitToType _toType;
        private readonly TransitFromType _fromType;

        public DiscardPileTypes()
        {
            _viewType = ViewType.DiscardPile;
            _toType = TransitToType.DiscardPile;
            _fromType = TransitFromType.DiscardPile;
        }

        public ViewType ViewType => _viewType;
        public TransitToType ToType => _toType;
        public TransitFromType FromType => _fromType;
    }
}
