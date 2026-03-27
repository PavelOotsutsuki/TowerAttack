using System.Collections;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers.Brothers;
using UnityEngine;

namespace GameFields.Effects
{
    public class BigBrotherEffect : Effect
    {
        private const int StartValue = 1;
        private const int UpgradeCount = 1;

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;

        public BigBrotherEffect(Person activePerson, BrothersEffectHandlerRoot brothersEffectHandlerRoot, EffectData data)
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

            Debug.Log("Эффект Большого брата закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            int countAttack = _activePerson.BrothersCounter;
            bool isAttackComplete = false;
            _activePerson.AttackActivate(StartValue + countAttack, () => isAttackComplete = true);
            yield return new WaitUntil(() => isAttackComplete);

            bool isDrawComplete = false;
            _drawCardManager.DrawCards(StartValue + countAttack, () => isDrawComplete = true);

            yield return new WaitUntil(() => isDrawComplete);

            _brothersEffectHandlerRoot.Upgrade(UpgradeCount);
        }
    }
}