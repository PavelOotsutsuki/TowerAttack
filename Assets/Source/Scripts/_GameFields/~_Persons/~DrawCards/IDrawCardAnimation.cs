using System.Collections.Generic;
using Cards;
using Tools;

namespace GameFields.Persons.DrawCards
{
    public interface IDrawCardAnimation: ICompletable
    {
        public void Play(IReadOnlyList<Card> cards);
    }
}