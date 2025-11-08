using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardCapabilityDescription
    {
        private const string AttackDescription = "<b>АТАКА: </b>Атакуйте картой замок, чтобы попытаться угадать номер карты противника. Если угадаете — победа.";
        private const string PlayDescription = "<b>РАЗЫГРАТЬ КАРТУ: </b>Положите карту на стол, чтобы разыграть её эффект.";
        private const string SearchDescription = "<b>ПОИСК: </b>Выберите номера карт, противник скажет находится ли одна из них у него в Замке.";
        private const string CurseDescription = "<b>ПРОКЛЯТЬЕ: </b>Соперник в начале хода игрока узнает случайное число, не являющееся его картой в Замке. Числа не повторяются.";
        private const string GnomeForgingDescription = "<b>ГНОМИЧЬЯ КОВКА: </b>Вы убираете карту в бито и берете одну карту из колоды. <b>Гномичей выбор</b> увеличивается на +2 карты.";
        private const string GnomeChoiceDescription = "<b>ГНОМИЧЕЙ ВЫБОР: </b>Вы <b>Ищете</b> несколько карт. Количество карт на <b>Поиск</b> увеличивается от <b>Гномичей ковки</b>. Каждый игрок может использовать этот навык только один раз за игру.";
        private const string VariantsDescription = "<b>ВАРИАНТ: </b>Эффект зависит от выбранного вами при розыгрыше варианта";
        private const string BrothersBondsDescription = "<b>БРАТСКИЕ УЗЫ: </b>Эффект братьев увеличивает от сыгранных карт братьев";
        private const string HandTransferDescription = "<b>ПЕРЕДАЧА: </b>Перенесите карту в руку противника, чтобы отдать её";

        private /*static*/ readonly CardCapability[] _cardCapabilities;
        private readonly IReadOnlyDictionary<CardCapability, CardCapabilityData> _capabilitiesDescription;

        private delegate bool GetDataAction(CardCapability checkCapability, CardCapability gettedCapability, out string result);

        public CardCapabilityDescription()
        {
            _cardCapabilities = Enum.GetValues(typeof(CardCapability)).Cast<CardCapability>().ToArray();

            _capabilitiesDescription = new Dictionary<CardCapability, CardCapabilityData>()
            {
                { CardCapability.Attack, new CardCapabilityData("АТАКА", AttackDescription)},
                { CardCapability.Play, new CardCapabilityData("РАЗЫГРАТЬ КАРТУ", PlayDescription)},
                { CardCapability.Search, new CardCapabilityData("ПОИСК", SearchDescription)},
                { CardCapability.Curse, new CardCapabilityData("ПРОКЛЯТЬЕ", CurseDescription)},
                { CardCapability.GnomeForging, new CardCapabilityData("ГНОМИЧЬЯ КОВКА", GnomeForgingDescription)},
                { CardCapability.GnomeChoice, new CardCapabilityData("ГНОМИЧЕЙ ВЫБОР", GnomeChoiceDescription)},
                { CardCapability.Variants, new CardCapabilityData("ВАРИАНТ", VariantsDescription)},
                { CardCapability.BrothersBonds, new CardCapabilityData("БРАТСКИЕ УЗЫ", BrothersBondsDescription)},
                { CardCapability.HandTransfer, new CardCapabilityData("ПЕРЕДАЧА", HandTransferDescription)}
            };
        }

        public string GetToStringValue(CardCapability cardCapability)
        {
            GetDataAction getDataAction = TryGetEnumToStringValue;

            return GetData(cardCapability, getDataAction);
        }

        public string GetDescription(CardCapability cardCapability)
        {
            GetDataAction getDataAction = TryGetDescriptionByNoComboEnumValue;

            return GetData(cardCapability, getDataAction);
        }

        private string GetData(CardCapability cardCapability, GetDataAction getDataAction)
        {
            string result = "";

            for (int i = 0; i < _cardCapabilities.Length; i++)
            {
                if (getDataAction.Invoke(_cardCapabilities[i], cardCapability, out string localResult))
                {
                    if (result != "")
                        result += "\n\n";

                    result += localResult;
                }
            }

            return result;
        }

        private bool TryGetEnumToStringValue(CardCapability checkCapability, CardCapability gettedCapability, out string result)
        {
            result = "";
            bool isCan = (gettedCapability & checkCapability) == checkCapability;

            if (isCan)
                result = _capabilitiesDescription[checkCapability].ToStringValue;

            return isCan;
        }

        private bool TryGetDescriptionByNoComboEnumValue(CardCapability checkCapability, CardCapability gettedCapability, out string result)
        {
            result = "";
            bool isCan = (gettedCapability & checkCapability) == checkCapability;

            if (isCan)
                result = _capabilitiesDescription[checkCapability].Description;

            return isCan;
        }
    }
}