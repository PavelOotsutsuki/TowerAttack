using System.Collections;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers.Brothers;
using UnityEngine;

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

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Малого брата закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            bool isDraw = false;
            int countCards = _activePerson.BrothersCounter;
            _drawCardManager.DrawCards(StartValue + countCards, () => isDraw = true);
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            yield return new WaitUntil(() => isDraw);

            _brothersEffectHandlerRoot.Upgrade(UpgradeCount);
            yield break;
            //_deactivePerson.AttackDeactivate();
        }
    }
}