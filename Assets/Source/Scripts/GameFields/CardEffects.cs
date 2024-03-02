using System.Collections;
using System.Collections.Generic;
using GameFields.Persons;
using UnityEngine;

namespace GameFields
{
    internal class CardEffects
    {
        private Deck _deck;
        private Player _player;
        private EnemyAI _enemyAI;

        public CardEffects(Deck deck, Player player, EnemyAI enemyAI)
        {
            _deck = deck;
            _player = player;
            _enemyAI = enemyAI;
        }

        public void PlayEffect()
        {

        }

    }
}
