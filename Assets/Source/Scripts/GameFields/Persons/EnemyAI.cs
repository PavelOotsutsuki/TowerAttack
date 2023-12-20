using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons
{
    public class EnemyAI : Person
    {
        protected override void DrawCard(Card drawnCard, Hand hand)
        {
            hand.AddCard(drawnCard);
        }
    }
}
