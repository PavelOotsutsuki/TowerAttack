using System.Collections.Generic;
using Tools.Settings;

namespace Tools.Utils.Orthographyes
{
    public static class Orthography
    {
        private static Dictionary<LanguageType, ILanguageRules> _rules;

        static Orthography()
        {
            _rules = new Dictionary<LanguageType, ILanguageRules>();
        }

        public static string GetWordByNumber(WordType wordType, int number, LanguageType? settedLanguage = null)
        {
            LanguageType language = settedLanguage == null ? GameSettings.Language : settedLanguage.Value;

            if (_rules.ContainsKey(language) == false)
            {
                CreateLanguageRules(language);
            }

            return _rules[language].GetWordByNumber(wordType, number);
        }

        public static string GetStringByNumber(int number, LanguageType? settedLanguage = null)
        {
            LanguageType language = settedLanguage == null ? GameSettings.Language : settedLanguage.Value;

            if (_rules.ContainsKey(language) == false)
            {
                CreateLanguageRules(language);
            }

            return _rules[language].GetStringByNumber(number);
        }

        private static void CreateLanguageRules(LanguageType language)
        {
            switch (language)
            {
                case LanguageType.RU:
                    _rules.Add(language, new RussianRules());
                    break;
                case LanguageType.EN:
                    _rules.Add(language, new EnglishRules());
                    break;
                default:
                    throw new System.Exception("Неизвестный LanguageType: " + language.ToString());
            }
        }
    }
}