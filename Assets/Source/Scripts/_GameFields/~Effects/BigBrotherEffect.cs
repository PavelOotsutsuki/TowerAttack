using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers.Brothers;

namespace GameFields.Effects
{
    public class BigBrotherEffect : Effect
    {
        private const int StartValue = 1;
        private const int UpgradeCount = 1;

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;

        public BigBrotherEffect(BrothersEffectHandlerRoot brothersEffectHandlerRoot, EffectData data)
            : base(data)
        {
            _activePerson = data.ActivePerson;
            _drawCardManager = data.ActivePerson;
            _brothersEffectHandlerRoot = brothersEffectHandlerRoot;

            Play();
        }

        protected override string GetName() => nameof(BigBrotherEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Большого брата закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            int countAttack = _activePerson.BrothersCounter;
            bool isAttackComplete = false;
            _activePerson.AttackActivate(StartValue + countAttack, () => isAttackComplete = true);
            await UniTask.WaitUntil(() => isAttackComplete, cancellationToken: Token);

            bool isDrawComplete = false;
            _drawCardManager.DrawCards(StartValue + countAttack, Token, () => isDrawComplete = true);

            await UniTask.WaitUntil(() => isDrawComplete, cancellationToken: Token);

            _brothersEffectHandlerRoot.Upgrade(UpgradeCount);
        }
    }
}