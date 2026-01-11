using Tools;

namespace Cards
{
    public class BigCardRootActivateData : IData
    {
        private readonly BigCardShowData _bigCardShowData;
        private readonly float _bigCardActivateDelay;
        private readonly float _bigCardDescriptionActivateDelay;

        public BigCardRootActivateData(BigCardShowData bigCardShowData, float bigCardActivateDelay,
            float bigCardDescriptionActivateDelay)
        {
            _bigCardShowData = bigCardShowData;
            _bigCardActivateDelay = bigCardActivateDelay;
            _bigCardDescriptionActivateDelay = bigCardDescriptionActivateDelay;
        }

        public BigCardShowData BigCardShowData => _bigCardShowData;
        public float BigCardActivateDelay => _bigCardActivateDelay;
        public float BigCardDescriptionActivateDelay => _bigCardDescriptionActivateDelay;
    }
}