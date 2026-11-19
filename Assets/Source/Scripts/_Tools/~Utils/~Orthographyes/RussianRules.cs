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

        public string GetStringByNumber(int number)
        {
            switch (number)
            {
                case 1:
                    return "один";
                case 2:
                    return "два";
                case 3:
                    return "три";
                case 4:
                    return "четыре";
                case 5:
                    return "пять";
                case 6:
                    return "шесть";
                case 7:
                    return "семь";
                case 8:
                    return "восемь";
                case 9:
                    return "девять";
                case 10:
                    return "десять";
                case 11:
                    return "одиннадцать";
                case 12:
                    return "двенадцать";
                case 13:
                    return "тринадцать";
                case 14:
                    return "четырнадцать";
                case 15:
                    return "пятнадцать";
                case 16:
                    return "шестнадцать";
                case 17:
                    return "семнадцать";
                case 18:
                    return "восемнадцать";
                case 19:
                    return "девятнадцать";
                case 20:
                    return "двадцать";
                case 21:
                    return "двадцать один";
                case 22:
                    return "двадцать два";
                case 23:
                    return "двадцать три";
                case 24:
                    return "двадцать четыре";
                case 25:
                    return "двадцать пять";
                case 26:
                    return "двадцать шесть";
                case 27:
                    return "двадцать семь";
                case 28:
                    return "двадцать восемь";
                case 29:
                    return "двадцать девять";
                case 30:
                    return "тридцать";
                case 31:
                    return "тридцать один";
                case 32:
                    return "тридцать два";
                case 33:
                    return "тридцать три";
                case 34:
                    return "тридцать четыре";
                case 35:
                    return "тридцать пять";
                case 36:
                    return "тридцать шесть";
                case 37:
                    return "тридцать семь";
                case 38:
                    return "тридцать восемь";
                case 39:
                    return "тридцать девять";
                case 40:
                    return "сорок";
                case 41:
                    return "сорок один";
                case 42:
                    return "сорок два";
                case 43:
                    return "сорок три";
                case 44:
                    return "сорок четыре";
                case 45:
                    return "сорок пять";
                case 46:
                    return "сорок шесть";
                case 47:
                    return "сорок семь";
                case 48:
                    return "сорок восемь";
                case 49:
                    return "сорок девять";
                case 50:
                    return "пятьдесят";
                case 51:
                    return "пятьдесят один";
                case 52:
                    return "пятьдесят два";
                case 53:
                    return "пятьдесят три";
                case 54:
                    return "пятьдесят четыре";
                case 55:
                    return "пятьдесят пять";
                case 56:
                    return "пятьдесят шесть";
                case 57:
                    return "пятьдесят семь";
                case 58:
                    return "пятьдесят восемь";
                case 59:
                    return "пятьдесят девять";
                case 60:
                    return "шестьдесят";
                case 61:
                    return "шестьдесят один";
                case 62:
                    return "шестьдесят два";
                case 63:
                    return "шестьдесят три";
                case 64:
                    return "шестьдесят четыре";
                case 65:
                    return "шестьдесят пять";
                case 66:
                    return "шестьдесят шесть";
                case 67:
                    return "шестьдесят семь";
                case 68:
                    return "шестьдесят восемь";
                case 69:
                    return "шестьдесят девять";
                case 70:
                    return "семьдесят";
                case 71:
                    return "семьдесят один";
                case 72:
                    return "семьдесят два";
                case 73:
                    return "семьдесят три";
                case 74:
                    return "семьдесят четыре";
                case 75:
                    return "семьдесят пять";
                case 76:
                    return "семьдесят шесть";
                case 77:
                    return "семьдесят семь";
                case 78:
                    return "семьдесят восемь";
                case 79:
                    return "семьдесят девять";
                case 80:
                    return "восемьдесят";
                case 81:
                    return "восемьдесят один";
                case 82:
                    return "восемьдесят два";
                case 83:
                    return "восемьдесят три";
                case 84:
                    return "восемьдесят четыре";
                case 85:
                    return "восемьдесят пять";
                case 86:
                    return "восемьдесят шесть";
                case 87:
                    return "восемьдесят семь";
                case 88:
                    return "восемьдесят восемь";
                case 89:
                    return "восемьдесят девять";
                case 90:
                    return "девяносто";
                case 91:
                    return "девяносто один";
                case 92:
                    return "девяносто два";
                case 93:
                    return "девяносто три";
                case 94:
                    return "девяносто четыре";
                case 95:
                    return "девяносто пять";
                case 96:
                    return "девяносто шесть";
                case 97:
                    return "девяносто семь";
                case 98:
                    return "девяносто восемь";
                case 99:
                    return "девяносто девять";
                case 100:
                    return "сто";
                default:
                    throw new System.Exception("Незаданное число: " + number);
            }
        }
    }
}