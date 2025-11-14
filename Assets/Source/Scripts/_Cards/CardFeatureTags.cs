using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Utils.Orthographyes;

namespace Cards
{
    internal class CardFeatureTags
    {
        private readonly string _featureTemplate;
        private readonly IEnumerable<TagValuePair> _tagValuePairs;
        private readonly IEnumerable<DefaultTagValuePair> _defaultTagValuePairs;

        public CardFeatureTags(string feature, IEnumerable<TagValuePair> tagValuePairs,
            IEnumerable<DefaultTagValuePair> defaultTagValuePairs)
        {
            _featureTemplate = feature;
            _tagValuePairs = tagValuePairs;
            _defaultTagValuePairs = defaultTagValuePairs;
        }

        public string CreateFeature(IEnumerable<TagValuePair> givenPairs = null)
        {
            givenPairs ??= _tagValuePairs;

            string result = _featureTemplate;

            foreach (DefaultTagValuePair pair in _defaultTagValuePairs) // Проверяем дефолтые замены (Capability цвета только пока что)
            {
                result = result.Replace($"<{pair.Tag}>", $"{pair.Value}");
            }

            foreach (TagValuePair pair in _tagValuePairs) // Потом проверяем теги которые могут Update-ится типо <CARDS_5>
            {
                TagValuePair givenPair = givenPairs.FirstOrDefault(p => p.Tag == pair.Tag);
                int currentValue = givenPair == null || givenPairs == _tagValuePairs ? pair.Value : givenPair.Value + pair.Value;

                string afterValue = "";

                if (Enum.TryParse(pair.Tag, false, out WordType wordType))
                {
                    afterValue += " ";
                    afterValue += Orthography.GetWordByNumber(wordType, currentValue);
                }

                result = result.Replace($"<{pair.Tag}_{pair.Value}>", $"<b>({currentValue})</b>{afterValue}");

            }

            return result;
        }

        public string UpdateTemplate(IEnumerable<TagValuePair> givenPairs)
        {
            givenPairs ??= _tagValuePairs;

            string result = _featureTemplate;

            if (givenPairs == _tagValuePairs)
                return result;

            foreach (TagValuePair pair in _tagValuePairs)
            {
                result = result.Replace($"<{pair.Tag}_{pair.Value}>", $"<{pair.Tag}_{givenPairs.First(p => p.Tag == pair.Tag)?.Value + pair.Value}>");
            }

            return result;
        }
    }
}