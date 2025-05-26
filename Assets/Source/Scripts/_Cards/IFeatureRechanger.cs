using System.Collections.Generic;

namespace Cards
{
    public interface IFeatureRechanger
    {
        public CardCapability CardCapability { get; }

        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs = null);
    }
}