using Tools;

namespace GameFields.CardTransits
{
    public class HandTypes : IData
    {
        private readonly ViewType _viewType;
        private readonly TransitToType _toType;
        private readonly TransitFromType _fromType;

        public HandTypes(ViewType viewType, TransitToType toType, TransitFromType fromType)
        {
            _viewType = viewType;
            _toType = toType;
            _fromType = fromType;
        }

        public ViewType ViewType => _viewType;
        public TransitToType ToType => _toType;
        public TransitFromType FromType => _fromType;
    }
}