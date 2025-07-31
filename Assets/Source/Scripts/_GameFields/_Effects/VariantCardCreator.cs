using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Effects
{
    public class VariantCardCreator : MonoBehaviour
    {
        [Header("22: FateMistress")]

        [SerializeField] private VariantCard _fateMistress_FatefulAttack_template;
        [SerializeField] private VariantCard _fateMistress_FateInevitability;

        //[Space]
        //[Header("----------------------------")]
        //[Space]

        private readonly Dictionary<EffectType, VariantCard[]> _effectsVariantsTemplates = new Dictionary<EffectType, VariantCard[]>(); 

        public void Init()
        {
            _effectsVariantsTemplates.Add(EffectType.FateMistress,
                new VariantCard[2]
                {
                    _fateMistress_FatefulAttack_template,
                    _fateMistress_FateInevitability
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