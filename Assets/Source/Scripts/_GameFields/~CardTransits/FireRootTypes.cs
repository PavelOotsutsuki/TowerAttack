using Tools;

namespace GameFields.CardTransits
{
    public class FireRootTypes : IData
    {
        private readonly ViewType _viewType;
        private readonly TransitFromType _fromType;

        public FireRootTypes()
        {
            _viewType = ViewType.FireRoot;
            _fromType = TransitFromType.FireRoot;
        }

        public ViewType ViewType => _viewType;
        public TransitFromType FromType => _fromType;
    }
}