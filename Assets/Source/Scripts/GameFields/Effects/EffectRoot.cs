using GameFields.DiscardPiles;
using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields.Effects
{
    public class EffectRoot : MonoBehaviour
    {
        [SerializeField] ZhyzhaEffect _zhyzhaEffect;
        [SerializeField] GreedyEffect _greedyEffect;
        [SerializeField] PatriarchCorallEffect _patriarchCorallEffect;

        public void Init(Deck deck, DiscardPile discardPile, IPerson activePerson, IPerson deactivePerson)
        {
            _zhyzhaEffect.Init(deactivePerson);
            _greedyEffect.Init(activePerson, deactivePerson);
            _patriarchCorallEffect.Init(deck, activePerson, deactivePerson);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineZhyzhaEffect();
            DefineGreedyEffect();
            DefinePatriarchCorallEffect();
        }

        [ContextMenu(nameof(DefineZhyzhaEffect))]
        private void DefineZhyzhaEffect()
        {
            AutomaticFillComponents.DefineComponent(this, ref _zhyzhaEffect, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineGreedyEffect))]
        private void DefineGreedyEffect()
        {
            AutomaticFillComponents.DefineComponent(this, ref _greedyEffect, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePatriarchCorallEffect))]
        private void DefinePatriarchCorallEffect()
        {
            AutomaticFillComponents.DefineComponent(this, ref _patriarchCorallEffect, ComponentLocationTypes.InChildren);
        }
    }
}
