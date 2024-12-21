using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public abstract class CardEffectProcessing : PersonStep
    {   
        private readonly Effect _effect;
        private bool _isComplete;

        public CardEffectProcessing(InteractionActivator interactionActivator, Effect effect): base(interactionActivator)
        {
            _isComplete = false;

            _effect = effect;
        }

        public override bool IsComplete => _isComplete;

        private bool IsEndEffect => _effect is null ? true : _effect.IsComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            WaitingEndEffect().ToUniTask();
        }

        private IEnumerator WaitingEndEffect()
        {
            yield return new WaitUntil(() => IsEndEffect);

            _isComplete = true;
        }
    }
}