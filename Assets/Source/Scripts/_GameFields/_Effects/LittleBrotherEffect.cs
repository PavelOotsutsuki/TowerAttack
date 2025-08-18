using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class LittleBrotherEffect : Effect
    {
        private const int StartValue = 1;

        private readonly Person _activePerson;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;

        private bool _endPlayingAttack;

        public LittleBrotherEffect(Person activePerson, BrothersEffectHandlerRoot brothersEffectHandlerRoot) : base()
        {
            _activePerson = activePerson;
            _brothersEffectHandlerRoot = brothersEffectHandlerRoot;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Малого брата закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlayingAttack = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            int countAttack = _activePerson.BrothersCounter;
            _activePerson.AttackActivate(StartValue + countAttack, EndPlayingCallback);
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);
            yield return new WaitUntil(() => _endPlayingAttack);

            _brothersEffectHandlerRoot.Upgrade(StartValue, 1);

            //_deactivePerson.AttackDeactivate();
        }

        private void EndPlayingCallback()
        {
            _endPlayingAttack = true;
        }
    }
}