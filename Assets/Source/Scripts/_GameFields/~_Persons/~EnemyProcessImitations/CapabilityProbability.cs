using Cards.Views;
using Tools;
using UnityEngine;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class CapabilityProbability : IData
    {
        private readonly CardCapability _cardCapability;
        private readonly int _probability;

        public CapabilityProbability(CardCapability cardCapability, int probability)
        {
            _cardCapability = cardCapability;
            _probability = probability;

            Debug.Log($"{_cardCapability}, {_probability}");
        }

        public CardCapability CardCapability => _cardCapability;
        public int Probability => _probability;
    }
}