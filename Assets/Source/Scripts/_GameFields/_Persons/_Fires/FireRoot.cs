using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools.Utils;
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
        public IEnumerable<Card> AllCards
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

            int thisCount = AllCards.Where(c => exceptions.Contains(c.ViewData.Number) == false).Count();

            return thisCount >= count;
        }

        public bool Contains(int number)
        {
            return AllCards.Select(c => c.ViewData.Number).Contains(number);
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            IReadOnlyList<Card> cards = Utils.Shuffle(AllCards);

            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (existingIndices.Contains(cards[i].ViewData.Number) == false && exceptions.Contains(cards[i].ViewData.Number) == false)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (exceptions.Contains(cards[i].ViewData.Number) == false)
                        {
                            result.Add(cards[i]);
                            existingIndices.Add(cards[i].ViewData.Number);
                        }
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
                throw new Exception("Ошибка вычисления чисел. Слишком мало карт!");

            return result;
        }
    }
}