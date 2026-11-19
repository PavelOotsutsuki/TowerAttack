using System;

namespace GameFields
{
    public interface ICardsCounter
    {
        public int CountCards { get; }
        public event Action OnSeatsCountChange;
    }
}