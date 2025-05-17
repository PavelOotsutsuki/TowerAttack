using System;
using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.CardTransits
{
    public interface IDrawCardManager
    {
        List<Card> DrawCards(int countCards, Action callback = null);
        Card DrawCard(Card card, Action callback = null);
    }
}