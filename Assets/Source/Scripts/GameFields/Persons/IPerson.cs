using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Persons
{
    public interface IPerson : IStartTowerCardSelectionListener
    {
        public void DiscardCards();
    }
}
