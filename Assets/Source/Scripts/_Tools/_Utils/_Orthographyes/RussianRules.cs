using System.Collections.Generic;
using UnityEngine;

namespace Tools.Utils.Orthographyes
{
    internal class RussianRules : ILanguageRules
    {
        private readonly IReadOnlyDictionary<WordType, RussianWordsEndings> _wordsRules; 

        public RussianRules()
        {
            _wordsRules = new Dictionary<WordType, RussianWordsEndings>()
            {
                { WordType.CARDS, new RussianWordsEndings("карту", "карты", "карт")},
                { WordType.NUMBERS, new RussianWordsEndings("номер", "номера", "номеров")},
                { WordType.TIME, new RussianWordsEndings("раз", "раза", "раз")}
            };
        }

        public string GetWordByNumber(WordType wordType, int number)
        {
            //if (wordType == WordType.CARDS || wordType == WordType.NUMBERS)
            //{
            //Debug.Log(number);

                if (number % 100 > 10 && number % 100 < 21)
                    return _wordsRules[wordType].DefaultEnding;

                if (number % 10 == 1)
                    return _wordsRules[wordType].OneEnding;

                if (number % 10 >= 2 && number % 10 <= 4)
                    return _wordsRules[wordType].TwoThreeFourEnding;
            //}

            //if (wordType == WordType.TIME)
            //{

            //}


            return _wordsRules[wordType].DefaultEnding;
        }
    }
}