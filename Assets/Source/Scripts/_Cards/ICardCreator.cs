using UnityEngine;

namespace Cards
{
    public interface ICardCreator
    {
        public Card CreateCard(CardName cardName, Transform parent);
    }
}