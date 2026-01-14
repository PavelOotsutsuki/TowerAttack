using Tools;

namespace Cards.Views.BigCardViews.CardDescriptions
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