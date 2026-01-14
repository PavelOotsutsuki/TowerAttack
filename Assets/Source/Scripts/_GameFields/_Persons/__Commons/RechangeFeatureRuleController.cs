using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Views;

namespace GameFields.Persons.Commons
{
    public class RechangeFeatureRuleController
    {
        private readonly Dictionary<CardCapability, IEnumerable<TagValuePair>> _rechangeFeatureRules;

        public RechangeFeatureRuleController()
        {
            _rechangeFeatureRules = new Dictionary<CardCapability, IEnumerable<TagValuePair>>();
        }

        public void Add(CardCapability effectFeature, IEnumerable<TagValuePair> tagValuePairs)
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
            CardCapability targetCapabiliry = card.CardCapability;
            List<TagValuePair> tagValuePairs = new List<TagValuePair>();

            foreach (CardCapability cardCapability in _rechangeFeatureRules.Keys)
            {
                if ((targetCapabiliry & cardCapability) == cardCapability)
                {
                    tagValuePairs.AddRange(_rechangeFeatureRules[cardCapability]);
                }
            }

            if (tagValuePairs.Count > 0)
            {
                card.RechangeFeature(tagValuePairs);
            }
            else
            {
                card.RechangeFeature();
            }

            //if (_rechangeFeatureRules.ContainsKey(targetCapabiliry))
            //{
            //    card.RechangeFeature(_rechangeFeatureRules[targetCapabiliry]);
            //}
            //else
            //{
            //    card.RechangeFeature();
            //}
        }
    }
}
