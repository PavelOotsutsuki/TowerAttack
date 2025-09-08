using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class BrothersMotherEffect : Effect
    {
        private const int UpgradeCount = 2;

        private readonly Person _activePerson;

        public BrothersMotherEffect(Person activePerson, SignalBus bus, CardEffectData data) : base(bus, data)
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