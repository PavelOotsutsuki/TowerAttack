using System;
using System.Collections.Generic;
using System.Threading;
using Cards;

namespace GameFields.Persons.DrawCards
{
    public interface IDrawCardManager
    {
        List<Card> DrawCards(int countCards, CancellationToken token, Action callback = null);
        int DrawCard(Card card, CancellationToken token, Action callback = null, int index = -1);
    }
}