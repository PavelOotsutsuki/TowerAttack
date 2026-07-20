using System.Collections.Generic;
using Humanizer;

namespace Tools.Utils.Orthographyes
{
    internal class EnglishRules : ILanguageRules
    {
        private readonly IReadOnlyDictionary<WordType, string> _defaultWords;

        public EnglishRules()
        {
            _defaultWords = new Dictionary<WordType, string>()
            {
                { WordType.CARDS, "card"},
                { WordType.NUMBERS, "number"}
            };
        }

        public string GetWordByNumber(WordType wordType, int number)
        {
            string result = _defaultWords[wordType];

            if (number != 1)
                result += "s";

            return result;
        }

        public string GetStringByNumber(int number)
        {
            return number.ToWords();
        }
    }
}