using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    internal abstract class CardActionProcessing : PersonStep
    {
        private readonly ICompletable _completable;
        private bool _isComplete;

        public CardActionProcessing(InteractionActivator interactionActivator, ICompletable completable, CancellationToken token) : base(interactionActivator, token)
        {
            _isComplete = false;

            _completable = completable;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            WaitingEndAttack().Forget();
        }

        private async UniTask WaitingEndAttack()
        {
            await UniTask.WaitUntil(() => _completable.IsComplete, cancellationToken: Token);

            _isComplete = true;
        }
    }
}