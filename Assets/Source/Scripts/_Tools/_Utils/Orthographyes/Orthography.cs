using System.Collections;
using System.Collections.Generic;
using Tools.Settings;
using UnityEngine;

namespace Tools.Utils.Orthographyes
{
    public static class Orthography
    {
        public static string GetWordByNumber(WordType wordType, int number, LanguageType language = GameSettings.Language)
        {
            switch (wordType)
            {
                case WordType.Numbers:
                    return GetNumbersWordByNumber(number, language);
                case WordType.Cards:
                    return GetCardsWordByNumber(number, language);
                default:
                    throw new System.Exception("Неизвестный WordType: " + wordType.ToString());
            }
        }

        #region Numbers
        private static string GetNumbersWordByNumber(int number, LanguageType language)
        {
            switch (language)
            {
                case LanguageType.RU:
                    return GetRussianNumbersWordByNumber(number);
                case LanguageType.EN:
                    return GetEnglishNumbersWordByNumber(number);
                default:
                    throw new System.Exception("Неизвестный LanguageType: " + language.ToString());
            }
        }

        private static string GetRussianNumbersWordByNumber(int number)
        {
            string result;

            switch (number)
            {
                case 1:
                case 21:
                case 31:
                case 41:
                    result = "номер";
                    break;
                case 2:
                case 3:
                case 4:
                case 22:
                case 23:
                case 24:
                case 32:
                case 33:
                case 34:
                case 42:
                case 43:
                case 44:
                    result = "номера";
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                default:
                    result = "номеров";
                    break;
            }

            return result;
        }

        private static string GetEnglishNumbersWordByNumber(int number)
        {
            if (number == 1)
                return "number";

            return "numbers";
        }
        #endregion

        #region Cards
        private static string GetCardsWordByNumber(int number, LanguageType language)
        {
            switch (language)
            {
                case LanguageType.RU:
                    return GetRussianCardsWordByNumber(number);
                case LanguageType.EN:
                    return GetEnglishCardsWordByNumber(number);
                default:
                    throw new System.Exception("Неизвестный LanguageType: " + language.ToString());
            }
        }

        private static string GetRussianCardsWordByNumber(int number)
        {
            string result;

            switch (number)
            {
                case 1:
                case 21:
                case 31:
                case 41:
                    result = "карта";
                    break;
                case 2:
                case 3:
                case 4:
                case 22:
                case 23:
                case 24:
                case 32:
                case 33:
                case 34:
                case 42:
                case 43:
                case 44:
                    result = "карты";
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                default:
                    result = "карт";
                    break;
            }

            return result;
        }

        private static string GetEnglishCardsWordByNumber(int number)
        {
            if (number == 1)
                return "card";

            return "cards";
        }
        #endregion 
    }
}