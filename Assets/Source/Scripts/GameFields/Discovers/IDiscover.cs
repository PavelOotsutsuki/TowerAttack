using Cards;
using System;
using System.Collections.Generic;

namespace GameFields.Persons.Discovers
{
    public interface IDiscover
    {
        public int MaxSeats { get; }

        public void Deactivate();
        public void Activate(List<Card> cards, string activateMessage, Action<Card> callback);
    }
}