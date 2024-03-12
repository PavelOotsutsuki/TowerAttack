using GameFields.DiscardPiles;
using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields.Effects
{
    public class EffectRoot : MonoBehaviour
    {
        //[SerializeField] Transform _transform;

        [SerializeField] ZhyzhaEffect _zhyzhaEffect;
        [SerializeField] GreedyEffect _greedyEffect;
        [SerializeField] PatriarchCorallEffect _patriarchCorallEffect;

        public void Init(Deck deck, DiscardPile discardPile, IPerson activePerson, IPerson deactivePerson)
        {
            //InstantiateAllEffects();

            _zhyzhaEffect.Init(deactivePerson);
            _greedyEffect.Init(activePerson, deactivePerson);
            _patriarchCorallEffect.Init(deck, activePerson, deactivePerson);
        }

        //private void InstantiateAllEffects()
        //{
        //    Instantiate(_zhyzhaEffect, _transform);
        //    Instantiate(_greedyEffect, _transform);
        //    Instantiate(_patriarchCorallEffect, _transform);
        //}

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            //DefineTransform();
            DefineZhyzhaEffect();
            DefineGreedyEffect();
            DefinePatriarchCorallEffect();
        }

        //[ContextMenu(nameof(DefineTransform))]
        //private void DefineTransform()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        //}

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
