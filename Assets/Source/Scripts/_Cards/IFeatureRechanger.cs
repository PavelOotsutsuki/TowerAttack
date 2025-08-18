using System.Collections.Generic;

namespace Cards
{
    public interface IFeatureRechanger
    {
        public EffectFeature EffectFeature { get; }

        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs = null);
    }
}