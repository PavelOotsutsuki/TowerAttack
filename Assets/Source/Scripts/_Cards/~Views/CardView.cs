using System;
using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Views
{
    [Serializable]
    public class CardView : IFeatureWatcher
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;

        private CardCapabilityDescription _cardCapabilityDescription;

        private CardFeatureTags _cardFeatureTags = null;
        private CardViewData _cardViewData = null;

        public string Feature => _feature.text;

        public void Init(CardCapabilityDescription cardCapabilityDescription)
        {
            _cardCapabilityDescription = cardCapabilityDescription;
        }

        public void FillData(CardViewData cardViewData)
        {
            _icon.sprite = cardViewData.Icon;
            _number.text = cardViewData.Number.ToString();
            _name.text = cardViewData.Name;
            _feature.text = cardViewData.Feature;

            if (TryFindTags(cardViewData.Feature))
            {
                _feature.text = _cardFeatureTags.CreateFeature();
                _cardViewData = cardViewData;
            }
            else
            {
                _feature.text = cardViewData.Feature;
            }
        }

        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs)
        {
            if (_cardFeatureTags != null)
            {
                _feature.text = _cardFeatureTags.CreateFeature(givenPairs);
                _cardViewData.ChangeFeature(_cardFeatureTags.UpdateTemplate(givenPairs));
            }
        }

        private bool TryFindTags(string feature)
        {
            if (feature.Contains('<') || feature.Contains('>'))
            {
                int beginTagsLength = feature.Split('<').Length;
                int endTagsLength = feature.Split('>').Length;

                if (beginTagsLength != endTagsLength)
                    throw new Exception("Неверно заданы теги в CardViewConfig (по количеству <>): " + feature);

                List<TagValuePair> result = new List<TagValuePair>();
                List<DefaultTagValuePair> defaultResult = new List<DefaultTagValuePair>();
                string featureCopy = feature;

                for (int i = 0; i < beginTagsLength - 1; i++)
                {
                    int start = featureCopy.IndexOf('<') + 1;
                    int end = featureCopy.IndexOf('>');
                    string tag = featureCopy.Substring(start, end - start);

                    if (tag == "b" || tag == "/b")
                    {
                        featureCopy = featureCopy.Substring(end + 1);
                        continue;
                    }

                    //if (tag == "color" || tag == "/color")
                    //{
                    //    featureCopy = featureCopy.Substring(end + 1);
                    //    continue;
                    //}

                    if (_cardCapabilityDescription.ContainsTag(tag)) // Значит дефолтный тег
                    {
                        defaultResult.Add(new DefaultTagValuePair(tag, _cardCapabilityDescription.GetCardFeatureText(tag)));
                        featureCopy = featureCopy.Substring(end + 1);
                        continue;
                    }

                    string[] tagParse = tag.Split('_');

                    if (tagParse.Length != 2)
                        throw new Exception("Неверно заданы теги в CardViewConfig (по количеству _): " + feature);

                    if (tagParse[0].ToUpper() != tagParse[0])
                        throw new Exception("Неверно заданы теги в CardViewConfig (необходимо чтобы тег был только из больших букв): " + feature);

                    if (int.TryParse(tagParse[1], out int defaultValue) == false)
                        throw new Exception("Неверно заданы теги в CardViewConfig (после _ должно идти целое число): " + feature);

                    result.Add(new TagValuePair(tagParse[0], defaultValue));
                    featureCopy = featureCopy.Substring(end + 1);
                }

                _cardFeatureTags = new CardFeatureTags(feature, result, defaultResult);
                return true;
            }

            return false;
        }
    }
}