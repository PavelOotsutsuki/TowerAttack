using GameFields.Persons;

namespace GameFields.CardTransits
{
    public class ViewTransitTypesRoot
    {
        private readonly PersonTypesManager _personTypesManager;
        private readonly DiscardPileTypes _discardPileTypes;
        private readonly DeckTypes _deckTypes;
        private readonly FireRootTypes _fireRootTypes;

        public ViewTransitTypesRoot()
        {
            _personTypesManager = new PersonTypesManager();
            _discardPileTypes = new DiscardPileTypes();
            _deckTypes = new DeckTypes();
            _fireRootTypes = new FireRootTypes();
        }

        public PersonTypes GetPersonTypes(IPersonObject owner)
        {
            return _personTypesManager.GetPersonTypes(owner);
        }

        public DiscardPileTypes DiscardPile => _discardPileTypes;
        public DeckTypes Deck => _deckTypes;
        public FireRootTypes FireRoot => _fireRootTypes;
    }
}