using System.Collections;
using GameFields.Persons;
using GameFields.Persons.EffectHandlers.Brothers;
using UnityEngine;

namespace GameFields.Effects
{
    public class MiddleBrotherEffect : Effect
    {
        private const int StartValue = 1;
        private const int UpgradeCount = 1;

        private readonly Person _activePerson;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;

        public MiddleBrotherEffect(Person activePerson, BrothersEffectHandlerRoot brothersEffectHandlerRoot,
            EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _brothersEffectHandlerRoot = brothersEffectHandlerRoot;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Среднего брата закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            bool endAttack = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            int countAttack = _activePerson.BrothersCounter;
            _activePerson.AttackActivate(StartValue + countAttack, () => endAttack = true);
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);
            yield return new WaitUntil(() => endAttack);

            _brothersEffectHandlerRoot.Upgrade(UpgradeCount);

            //_deactivePerson.AttackDeactivate();
        }
    }
}