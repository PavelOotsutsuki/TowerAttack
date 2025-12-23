using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Utils.Orthographyes;

namespace Cards
{
    public class CardFeatureTags
    {
        private readonly string _featureTemplate;
        private readonly IEnumerable<TagValuePair> _tagValuePairs;

        public CardFeatureTags(string feature, IEnumerable<TagValuePair> tagValuePairs)
        {
            _featureTemplate = feature;
            _tagValuePairs = tagValuePairs;
        }

        public string CreateFeature(IEnumerable<TagValuePair> givenPairs = null)
        {
            givenPairs ??= _tagValuePairs;

            string result = _featureTemplate;

            foreach (TagValuePair pair in _tagValuePairs)
            {
                TagValuePair givenPair = givenPairs.FirstOrDefault(p => p.Tag == pair.Tag);
                //int currentValue = givenPair == null ? pair.Value : givenPair.Value;
                int currentValue = givenPair == null || givenPairs == _tagValuePairs ? pair.Value : givenPair.Value + pair.Value;

                string afterValue = "";

                if (Enum.TryParse(pair.Tag, false, out WordType wordType))
                {
                    afterValue += " ";
                    afterValue += Orthography.GetWordByNumber(wordType, pair.Value);
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