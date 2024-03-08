using System.Collections;
using System.Collections.Generic;
using GameFields.DiscardPiles;
using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields.Effects
{
    public class EffectRoot : MonoBehaviour
    {
        [SerializeField] PatriarchCorallEffect _patriarchCorallEffect;

        public void Init(Deck deck, DiscardPile discardPile, IPerson activePerson, IPerson deactivePerson)
        {
            _patriarchCorallEffect.Init(deck, activePerson, deactivePerson);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefinePatriarchCorallEffect();
        }

        [ContextMenu(nameof(DefinePatriarchCorallEffect))]
        private void DefinePatriarchCorallEffect()
        {
            AutomaticFillComponents.DefineComponent(this, ref _patriarchCorallEffect, ComponentLocationTypes.InThis);
        }
    }
}
