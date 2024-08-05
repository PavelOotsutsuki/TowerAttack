using Cards;

namespace GameFields.Persons.Hands
{
    public class HandPlayer : Hand
    {
        public override void AddCard(Card card)
        {
            card.SetDragAndDropListener(this);

            base.AddCard(card);
        }
    }
}
