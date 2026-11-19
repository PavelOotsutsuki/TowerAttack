using Tools;

namespace GameFields.CardTransits
{
    public class PersonTypes : IData
    {
        private readonly HandTypes _handTypes;
        private readonly TransitToType _firePool;
        private readonly ViewType _table;

        public PersonTypes(HandTypes handTypes, TransitToType firePool, ViewType table)
        {
            _handTypes = handTypes;
            _firePool = firePool;
            _table = table;
        }

        public HandTypes Hand => _handTypes;
        public TransitToType FirePool => _firePool;
        public ViewType Table => _table;
    }
}