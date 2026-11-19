using System.Collections.Generic;
using System.Threading;
using Cards;
using Tools;

namespace GameFields.Persons.DrawCards
{
    public interface IDrawCardAnimation: ICompletable
    {
        public void Play(IReadOnlyList<Card> cards, int indexAdd, CancellationToken token);
    }
}