using System.Collections.Generic;
using Cards;
using Cards.Views;
using Tools.Utils;
using Random = UnityEngine.Random;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class EasyAIThinkLogic : IAIThinkLogic
    {
        public EasyAIThinkLogic()
        { }

        public CapabilityProbability FindActionType(Card workCard)
        {
            CardCapability cardCapability = workCard.CardCapability;

            List<CardCapability> testflags = Utils.GetFlags(cardCapability & (CardCapability.Play |
                CardCapability.GnomeForging | CardCapability.HandTransfer));

            if (testflags.Count == 0)
                return new CapabilityProbability(CardCapability.Attack, 100);

            return new CapabilityProbability(testflags[Random.Range(0, testflags.Count)], 100);
        }
    }
}