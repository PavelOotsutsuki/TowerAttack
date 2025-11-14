using System.Collections;
using Cards;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.SelectMenues;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class LittleBrotherEffect : Effect
    {
        private const int StartValue = 1;
        private const int UpgradeCount = 1;

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;

        public LittleBrotherEffect(Person activePerson, BrothersEffectHandlerRoot brothersEffectHandlerRoot, EffectData data)
            : base(data)
        {
            _activePerson = activePerson;
            _drawCardManager = activePerson;
            _brothersEffectHandlerRoot = brothersEffectHandlerRoot;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Малого брата закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            int countCards = _activePerson.BrothersCounter;
            _drawCardManager.DrawCards(StartValue + countCards);
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);
            _brothersEffectHandlerRoot.Upgrade(UpgradeCount);
            yield break;
            //_deactivePerson.AttackDeactivate();
        }
    }
}