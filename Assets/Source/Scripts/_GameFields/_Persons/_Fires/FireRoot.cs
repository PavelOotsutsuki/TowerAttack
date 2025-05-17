using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class FireRoot: ICardView
    {
        private readonly FirePool _playerFirePool;
        private readonly FirePool _enemyFirePool;

        public FireRoot(FirePool playerFirePool, FirePool enemyFirePool)
        {
            _playerFirePool = playerFirePool;
            _enemyFirePool = enemyFirePool;
        }

        public int Count => _playerFirePool.Count + _enemyFirePool.Count;
        public IReadOnlyList<Card> Cards
        {
            get
            {
                List<Card> allCards = new List<Card>();

                allCards.AddRange(_playerFirePool.FireList);
                allCards.AddRange(_enemyFirePool.FireList);

                return allCards;
            }
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            int thisCount = Cards.Where(c => exceptions.Contains(c.ViewConfig.Number) == false).Count();

            return thisCount >= count;
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, Count);

                while (existingIndices.Contains(randomIndex) || exceptions.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, Count);
                }

                result.Add(Cards[randomIndex]);
            }

            return result;
        }
    }
}