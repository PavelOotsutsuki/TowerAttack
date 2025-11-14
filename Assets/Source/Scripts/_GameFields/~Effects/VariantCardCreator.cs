using System;
using System.Collections.Generic;
using Cards.Effects;
using Cards.Views;
using UnityEngine;

namespace GameFields.Effects
{
    public class VariantCardCreator : MonoBehaviour
    {
        [Header("22: FateMistress")]

        [SerializeField] private VariantCard _fateMistress_FatefulAttack_template;
        [SerializeField] private VariantCard _fateMistress_FateInevitability_template;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("25: JusticeBull")]

        [SerializeField] private VariantCard _justiceBull_SmallerOnesArmy_template;
        [SerializeField] private VariantCard _justiceBull_BigOnesArmy_template;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("34: HungryOgre")]

        [SerializeField] private VariantCard _hungryOgre_SilentSearch_template;
        [SerializeField] private VariantCard _hungryOgre_HighProfileCrime_template;

        private readonly Dictionary<EffectType, VariantCard[]> _effectsVariantsTemplates = new Dictionary<EffectType, VariantCard[]>(); 

        public void Init()
        {
            _effectsVariantsTemplates.Add(EffectType.FateMistress,
                new VariantCard[2]
                {
                    _fateMistress_FatefulAttack_template,
                    _fateMistress_FateInevitability_template
                });

            _effectsVariantsTemplates.Add(EffectType.JusticeBull,
                new VariantCard[2]
                {
                    _justiceBull_SmallerOnesArmy_template,
                    _justiceBull_BigOnesArmy_template
                });

            _effectsVariantsTemplates.Add(EffectType.HungryOgre,
                new VariantCard[2]
                {
                    _hungryOgre_SilentSearch_template,
                    _hungryOgre_HighProfileCrime_template
                });
        }

        public IReadOnlyList<VariantCard> CreateByEffect(EffectType type)
        {
            if (_effectsVariantsTemplates.ContainsKey(type) == false)
                throw new NullReferenceException("Variants is not founded");

            return CreateVariantsByTemplates(_effectsVariantsTemplates[type]);
        }

        private IReadOnlyList<VariantCard> CreateVariantsByTemplates(VariantCard[] templates)
        {
            List<VariantCard> variantCards = new List<VariantCard>();

            foreach (VariantCard template in templates)
            {
                VariantCard variantCard = Instantiate(template);
                variantCard.Init();
                variantCards.Add(variantCard);
            }

            return variantCards;
        }
    }
}