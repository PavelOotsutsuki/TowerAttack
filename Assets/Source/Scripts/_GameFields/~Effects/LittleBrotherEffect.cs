using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers.Brothers;

namespace GameFields.Effects
{
    public class LittleBrotherEffect : Effect
    {
        private const int StartValue = 1;
        private const int UpgradeCount = 1;

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;

        public LittleBrotherEffect(BrothersEffectHandlerRoot brothersEffectHandlerRoot, EffectData data)
            : base(data)
        {
            _activePerson = data.ActivePerson;
            _drawCardManager = data.ActivePerson;
            _brothersEffectHandlerRoot = brothersEffectHandlerRoot;

            Play();
        }

        protected override string GetName() => nameof(LittleBrotherEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Малого брата закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            bool isDraw = false;
            int countCards = _activePerson.BrothersCounter;
            _drawCardManager.DrawCards(StartValue + countCards, Token, () => isDraw = true);
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            await UniTask.WaitUntil(() => isDraw, cancellationToken: Token);

            _brothersEffectHandlerRoot.Upgrade(UpgradeCount);
            //_deactivePerson.AttackDeactivate();
        }
    }
}