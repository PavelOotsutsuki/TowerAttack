using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public class CardEffectProcessing : PersonStep
    {   
        private Effect _currentEffect;
        private bool _isComplete;

        public CardEffectProcessing(GameFieldObjectsActivator gameFieldObjectsActivator): base(gameFieldObjectsActivator)
        {
            _isComplete = false;

            _currentEffect = null;
        }

        public void SetEffect(Effect effect)
        {
            _currentEffect = effect;
        }

        public override bool IsComplete => _isComplete;

        private bool IsEndEffect => _currentEffect is null ? true : _currentEffect.IsComplete;

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