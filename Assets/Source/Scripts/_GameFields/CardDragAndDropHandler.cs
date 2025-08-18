using Cards;
using GameFields.Persons.Hands;
using GameFields.LightControls;
using CanvasSortOrders;

namespace GameFields
{
    public class CardDragAndDropHandler : ICardDragAndDropHandler, ICardDragAndDropBlockable
    {
        private readonly ICardDragAndDropHandHandler _cardDragAndDropHandPlayer;
        private readonly IHandBlockable _handPlayerBlockable; 
        private readonly CardDragAndDropLightController _cardDragAndDropLightController;
        private readonly CanvasSortOrder _sortOrder;

        public CardDragAndDropHandler(ICardDragAndDropHandHandler cardDragAndDropHandPlayer, IHandBlockable handPlayerBlockable,
            CardDragAndDropLightController cardDragAndDropLightController, SpeedUpButtonSortOrder sortOrder)
        {
            _cardDragAndDropHandPlayer = cardDragAndDropHandPlayer;
            _handPlayerBlockable = handPlayerBlockable;
            _cardDragAndDropLightController = cardDragAndDropLightController;
            _sortOrder = sortOrder;
        }

        public float ReturnInSeatDuration => _cardDragAndDropHandPlayer.ReturnInSeatDuration;

        public bool IsDraggable(Card card) => _cardDragAndDropHandPlayer.IsDraggable(card);

        public void OnCardAttack()
        {
            _cardDragAndDropHandPlayer.OnCardAttack();
            _cardDragAndDropLightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void OnCardDrag(Card card)
        {
            _cardDragAndDropHandPlayer.OnCardDrag(card);
            CardDragAndDropLightControllerActivateData lightActivateData = new CardDragAndDropLightControllerActivateData(card.EffectType);
            _cardDragAndDropLightController.Activate(lightActivateData);
            _sortOrder.SetSortIndex(-1);
        }

        public void OnCardDrop()
        {
            _cardDragAndDropHandPlayer.OnCardDrop();
            _cardDragAndDropLightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void OnCardPlay()
        {
            _cardDragAndDropHandPlayer.OnCardPlay();
            _cardDragAndDropLightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void OnCardReturnInHand(Card card)
        {
            _cardDragAndDropHandPlayer.OnCardReturnInHand(card);
        }

        public void ForciblyBlock()
        {
            _handPlayerBlockable.ForciblyBlock();
            _cardDragAndDropLightController.Deactivate();
            _sortOrder.SetDefaultIndex();
        }

        public void Unblock()
        {
            _handPlayerBlockable.Unblock();
        }
    }
}