using System;
using System.Collections.Generic;
using Tools;

namespace Cards
{
    public class CardCapabilityLookUp : IData
    {
        private readonly Dictionary<CardCapability, CardCapabilityData> _dataByEnum;
        private readonly Dictionary<string, string> _textByTag;

        public CardCapabilityLookUp()
        {
            _dataByEnum = new Dictionary<CardCapability, CardCapabilityData>();
            _textByTag = new Dictionary<string, string>();
        }

        public void Add(CardCapability cardCapability, string tag, string toStringValue,
            string description, string colorTag)
        {
            _dataByEnum.Add(cardCapability, new CardCapabilityData(toStringValue, description, colorTag));

            _textByTag.Add(tag, $"<b><color=#{colorTag}>");
            _textByTag.Add($"/{tag}", "</color></b>");
        }

        public bool ContainsTag(string tag) => _textByTag.ContainsKey(tag);

        public string GetToStringValue(CardCapability cardCapability)
        {
            CheckCardCapability(cardCapability);

            return _dataByEnum[cardCapability].ToStringValue;
        }

        public string GetDescription(CardCapability cardCapability)
        {
            CheckCardCapability(cardCapability);

            return _dataByEnum[cardCapability].ToStringValue.ToUpper() + ": " + _dataByEnum[cardCapability].Description;
        }

        public string GetTextByTag(string tag)
        {
            CheckTag(tag);

            return _textByTag[tag];
        }

        private void CheckCardCapability(CardCapability cardCapability)
        {
            if (_dataByEnum.ContainsKey(cardCapability) == false)
                throw new Exception($"Неизвестный CardCapability: {cardCapability}");
        }

        private void CheckTag(string tag)
        {
            if (_textByTag.ContainsKey(tag) == false)
                throw new Exception($"Неизвестный tag для CardCapability: {tag}");
        }
    }
}