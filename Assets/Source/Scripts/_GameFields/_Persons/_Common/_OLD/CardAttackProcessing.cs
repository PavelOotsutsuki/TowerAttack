using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Common.OLD
{
    public abstract class CardAttackProcessing : PersonStep
    {
        private readonly ICompletable _completable;
        private bool _isComplete;

        public CardAttackProcessing(InteractionActivator interactionActivator, ICompletable completable) : base(interactionActivator)
        {
            _isComplete = false;

            _completable = completable;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            WaitingEndAttack().ToUniTask();
        }

        private IEnumerator WaitingEndAttack()
        {
            yield return new WaitUntil(() => _completable.IsComplete);

            _isComplete = true;
        }
    }
}