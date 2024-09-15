using System;
using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.CardTransits
{
    public interface IDrawCardManager
    {
        public List<Card> DrawCards(int countCards, Action callback = null);
    }
}