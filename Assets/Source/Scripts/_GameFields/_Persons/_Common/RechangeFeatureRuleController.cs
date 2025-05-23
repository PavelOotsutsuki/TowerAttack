using System.Collections.Generic;
using System.Linq;
using Cards;

namespace GameFields.Persons.Common
{
    public class RechangeFeatureRuleController
    {
        private readonly Dictionary<EffectFeature, IEnumerable<TagValuePair>> _rechangeFeatureRules;

        public RechangeFeatureRuleController()
        {
            _rechangeFeatureRules = new Dictionary<EffectFeature, IEnumerable<TagValuePair>>();
        }

        public void Add(EffectFeature effectFeature, IEnumerable<TagValuePair> tagValuePairs)
        {
            if (_rechangeFeatureRules.ContainsKey(effectFeature) == false)
            {
                _rechangeFeatureRules.Add(effectFeature, tagValuePairs);
                return;
            }

            List<TagValuePair> resultPairs = new List<TagValuePair>();

            foreach (TagValuePair existPair in _rechangeFeatureRules[effectFeature])
            {
                if (tagValuePairs.Select(tvp => tvp.Tag).Contains(existPair.Tag) == false)
                    resultPairs.Add(existPair);
            }

            resultPairs.AddRange(tagValuePairs);

            _rechangeFeatureRules.Remove(effectFeature);
            _rechangeFeatureRules.Add(effectFeature, resultPairs);
        }

        public void TryRenameFeature(IFeatureRechanger card)
        {
            EffectFeature effectFeature = card.EffectFeature;

            if (_rechangeFeatureRules.ContainsKey(effectFeature))
            {
                card.RechangeFeature(_rechangeFeatureRules[effectFeature]);
            }
            else
            {
                card.RechangeFeature();
            }
        }
    }
}
