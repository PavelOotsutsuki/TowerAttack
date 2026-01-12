using System.Collections;
using Cards;
using GameFields.Persons;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.SelectMenues;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class BrothersMotherEffect : Effect
    {
        private const int UpgradeCount = 2;

        private readonly Person _activePerson;

        public BrothersMotherEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Матери братьев закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.UpgradeBrothers(UpgradeCount);
            yield break;
        }
    }
}