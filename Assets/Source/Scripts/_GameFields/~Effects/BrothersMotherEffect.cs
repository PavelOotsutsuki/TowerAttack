using System.Collections;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class BrothersMotherEffect : Effect
    {
        private const int UpgradeCount = 2;

        private readonly Person _activePerson;

        public BrothersMotherEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Матери братьев закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            _activePerson.UpgradeBrothers(UpgradeCount);
            yield break;
        }
    }
}