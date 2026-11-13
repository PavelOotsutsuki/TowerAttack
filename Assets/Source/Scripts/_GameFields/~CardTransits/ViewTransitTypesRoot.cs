using System;
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

        public ViewType ConvertToViewType(TransitFromType transitFromType)
        {
            switch (transitFromType)
            {
                case TransitFromType.DiscardPile:
                    return ViewType.DiscardPile;
                case TransitFromType.FireRoot:
                    return ViewType.FireRoot;
                case TransitFromType.HandEnemy:
                    return ViewType.HandAI;
                case TransitFromType.HandPlayer:
                    return ViewType.HandPlayer;
                default:
                    throw new Exception($"Ошибка конвертации типов. Неизвестный тип {nameof(TransitFromType)}: {transitFromType}");
            }
        }

        public ViewType ConvertToViewType(TransitToType transitToType)
        {
            switch (transitToType)
            {
                case TransitToType.Deck:
                    return ViewType.Deck;
                case TransitToType.DiscardPile:
                    return ViewType.DiscardPile;
                case TransitToType.EnemyFirePool:
                    return ViewType.FireRoot;
                case TransitToType.PlayerFirePool:
                    return ViewType.FireRoot;
                case TransitToType.HandEnemy:
                    return ViewType.HandAI;
                case TransitToType.HandPlayer:
                    return ViewType.HandPlayer;
                default:
                    throw new Exception($"Ошибка конвертации типов. Неизвестный тип {nameof(TransitToType)}: {transitToType}");
            }
        }
    }
}