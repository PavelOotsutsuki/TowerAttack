using System.Collections.Generic;
using System.Linq;


namespace Cards
{
    public class CardFeatureTags
    {
        private readonly string _feature;
        private readonly IReadOnlyList<TagValuePair> _tagValuePairs;

        public CardFeatureTags(string feature, IReadOnlyList<TagValuePair> tagValuePairs)
        {
            _feature = feature;
            _tagValuePairs = tagValuePairs;
        }

        public string CreateFeature(IReadOnlyList<TagValuePair> givenPairs = null)
        {
            givenPairs ??= _tagValuePairs;

            string result = _feature;

            foreach (TagValuePair pair in _tagValuePairs)
            {
                result = result.Replace($"<{pair.Tag}_{pair.Value}>", $"({givenPairs.First(p => p.Tag == pair.Tag).Value})");
            }

            return result;
        }
    }
}