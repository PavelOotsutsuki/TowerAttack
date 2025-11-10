using Tools;

namespace Cards
{
    public class BigCardRootActivateData : IData
    {
        private readonly BigCardShowData _bigCardShowData;
        private readonly BigCardViewType _viewType;

        public BigCardRootActivateData(BigCardShowData bigCardShowData, BigCardViewType viewType)
        {
            _bigCardShowData = bigCardShowData;
            _viewType = viewType;
        }

        public BigCardShowData BigCardShowData => _bigCardShowData;
        public BigCardViewType ViewType => _viewType;
    }
}