using System;
using Cards;

namespace GameFields.Persons.Towers
{
    public interface ICopyCardCreator
    {
        void CreateCopyCard(Action<Card> insertedCardCallback);
    }
}