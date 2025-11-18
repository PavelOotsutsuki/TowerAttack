using Tools;

namespace Cards
{
    public class CardDescriptionActivateData : IData
    {
        private readonly string _description;

        public CardDescriptionActivateData(string description)
        {
            _description = description;
        }

        public string Description => _description;
    }
}