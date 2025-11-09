using System;
using System.Collections.Generic;
using System.Linq;

namespace Cards
{
    public class CardCapabilityDescription
    {
        private const string AttackDescription = "Атакуйте картой замок, чтобы попытаться угадать номер карты противника. Если угадаете — победа.";
        private const string PlayDescription = "Положите карту на стол, чтобы разыграть её эффект.";
        private const string SearchDescription = "Выберите номера карт, противник скажет находится ли одна из них у него в Замке.";
        private const string CurseDescription = "Соперник в начале хода игрока узнает случайное число, не являющееся его картой в Замке. Числа не повторяются.";
        private const string GnomeForgingDescription = "Вы убираете карту в бито и берете одну карту из колоды. <b>Гномичей выбор</b> увеличивается на +2 карты.";
        private const string GnomeChoiceDescription = "Вы <b>Ищете</b> несколько карт. Количество карт на <b>Поиск</b> увеличивается от <b>Гномичей ковки</b>. Каждый игрок может использовать этот навык только один раз за игру.";
        private const string VariantsDescription = "Эффект зависит от выбранного вами при розыгрыше варианта";
        private const string BrothersBondsDescription = "Эффект братьев увеличивает от сыгранных карт братьев";
        private const string HandTransferDescription = "Перенесите карту в руку противника, чтобы отдать её";

        private const string RedColorTag = "FF0000";
        private const string GreenColorTag = "00A107";
        private const string BlueColorTag = "0000FF";
        private const string BlackColorTag = "000000";
        private const string OrangeColorTag = "FF5200";
        private const string BlackBlueColorTag = "233D8A";
        private const string YellowColorTag = "B58E0F";
        private const string PurpleColorTag = "9F0F82";
        private const string GrayColorTag = "6F5D6B";

        private readonly string AttackColorTag = RedColorTag;
        private readonly string PlayColorTag = GreenColorTag;
        private readonly string SearchColorTag = BlueColorTag;
        private readonly string CurseColorTag = BlackColorTag;
        private readonly string GnomeForgingColorTag = OrangeColorTag;
        private readonly string GnomeChoiceColorTag = BlackBlueColorTag;
        private readonly string VariantsColorTag = YellowColorTag;
        private readonly string BrothersBondsColorTag = PurpleColorTag;
        private readonly string HandTransferColorTag = GrayColorTag;

        private /*static*/ readonly CardCapability[] _cardCapabilities;
        //private readonly IReadOnlyDictionary<CardCapability, CardCapabilityData> _capabilitiesDescription;
        private readonly CardCapabilityLookUp _cardCapabilityLookUp;

        public CardCapabilityDescription()
        {
            _cardCapabilities = Enum.GetValues(typeof(CardCapability)).Cast<CardCapability>().ToArray();

            _cardCapabilityLookUp = new CardCapabilityLookUp();

            _cardCapabilityLookUp.Add(CardCapability.Attack, "CAP_1", "Атака", AttackDescription, AttackColorTag);
            _cardCapabilityLookUp.Add(CardCapability.Play, "CAP_2", "Разыграть карту", PlayDescription, PlayColorTag);
            _cardCapabilityLookUp.Add(CardCapability.Search, "CAP_3", "Поиск", SearchDescription, SearchColorTag);
            _cardCapabilityLookUp.Add(CardCapability.Curse, "CAP_4", "Проклятье", CurseDescription, CurseColorTag);
            _cardCapabilityLookUp.Add(CardCapability.GnomeForging, "CAP_5", "Гномичья ковка", GnomeForgingDescription,
                GnomeForgingColorTag);
            _cardCapabilityLookUp.Add(CardCapability.GnomeChoice, "CAP_6", "Гномичей выбор", GnomeChoiceDescription,
                GnomeChoiceColorTag);
            _cardCapabilityLookUp.Add(CardCapability.Variants, "CAP_7", "Вариант", VariantsDescription, VariantsColorTag);
            _cardCapabilityLookUp.Add(CardCapability.BrothersBonds, "CAP_8", "Братские узы", BrothersBondsDescription,
                BrothersBondsColorTag);
            _cardCapabilityLookUp.Add(CardCapability.HandTransfer, "CAP_9", "Передача", HandTransferDescription,
                HandTransferColorTag);
        }

        public bool ContainsTag(string tag) => _cardCapabilityLookUp.ContainsTag(tag);

        public string GetCardFeatureText(string tag)
        {
            return _cardCapabilityLookUp.GetTextByTag(tag);
        }

        public string GetAllCapabilitiesToStringValue(CardCapability cardCapability)
        {
            string result = "";
            int valueCounter = 1;

            for (int i = 0; i < _cardCapabilities.Length; i++)
            {
                if (TryGetEnumToStringValue(_cardCapabilities[i], cardCapability, out string localResult))
                {
                    localResult = valueCounter++.ToString() + ". " + localResult;
                    result = ConcatByParagraphStyle(result, localResult);
                }
            }

            if (result != "")
                result = "Свойства:\n" + result;

            return result;
        }

        public string GetAllCapabilitiesDescription(CardCapability cardCapability)
        {
            string result = "";

            for (int i = 0; i < _cardCapabilities.Length; i++)
            {
                if (TryGetDescriptionByNoComboEnumValue(_cardCapabilities[i], cardCapability, out string localResult))
                {
                    result = ConcatByDoubleParagraphStyle(result, localResult);
                }
            }

            return result;
        }

        private string ConcatByDoubleParagraphStyle(string result, string localResult)
        {
            if (result != "")
                result += "\n\n";

            result += localResult;

            return result;
        }

        private string ConcatByParagraphStyle(string result, string localResult)
        {
            if (result != "")
                result += "\n";

            result += localResult;

            return result;
        }

        private bool TryGetEnumToStringValue(CardCapability checkCapability, CardCapability gettedCapability, out string result)
        {
            result = "";
            bool isCan = (gettedCapability & checkCapability) == checkCapability;

            if (isCan)
            {
                result = _cardCapabilityLookUp.GetToStringValue(checkCapability);
            }

            return isCan;
        }

        private bool TryGetDescriptionByNoComboEnumValue(CardCapability checkCapability, CardCapability gettedCapability, out string result)
        {
            result = "";
            bool isCan = (gettedCapability & checkCapability) == checkCapability;

            if (isCan)
                result = _cardCapabilityLookUp.GetDescription(checkCapability);

            return isCan;
        }
    }
}