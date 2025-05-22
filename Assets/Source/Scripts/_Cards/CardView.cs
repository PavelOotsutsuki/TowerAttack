using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    [Serializable]
    public class CardView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;

        private CardFeatureTags _cardFeatureTags = null;

        public void FillData(CardViewConfig cardViewConfig)
        {
            _icon.sprite = cardViewConfig.Icon;
            _number.text = cardViewConfig.Number.ToString();
            _name.text = cardViewConfig.Name;

            if (TryFindTags(cardViewConfig.Feature))
            {
                _feature.text = _cardFeatureTags.CreateFeature();
            }
            else
            {
                _feature.text = cardViewConfig.Feature;
            }
        }

        public void RechangeFeature(IReadOnlyList<TagValuePair> givenPairs)
        {
            if (_cardFeatureTags != null)
                _feature.text = _cardFeatureTags.CreateFeature(givenPairs);
        }

        private bool TryFindTags(string feature)
        {
            if (feature.Contains('<') || feature.Contains('>'))
            {
                int beginTagsLength = feature.Split('<').Length;
                int endTagsLength = feature.Split('>').Length;

                if (beginTagsLength != endTagsLength)
                    throw new Exception("Неверно заданы теги в CardViewConfig (по количеству <>): " + feature);

                List<TagValuePair> result = new List<TagValuePair>(beginTagsLength - 1);
                string featureCopy = feature;

                for (int i = 0; i < beginTagsLength - 1; i++)
                {
                    int start = featureCopy.IndexOf('<') + 1;
                    int end = featureCopy.IndexOf('>');
                    string tag = featureCopy.Substring(start, end - start);

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

                _cardFeatureTags = new CardFeatureTags(feature, result);
                return true;
            }

            return false;
        }
    }
}