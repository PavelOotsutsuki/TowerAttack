using System.Collections;
using System.Collections.Generic;
using Cards;
using Hands;
using UnityEngine;

namespace Fights.Persons
{
    public class EnemyAI : Person
    {
        protected override void DrawCard(Card drawnCard, Hand hand)
        {
            hand.AddCard(drawnCard);
        }
    }
}
