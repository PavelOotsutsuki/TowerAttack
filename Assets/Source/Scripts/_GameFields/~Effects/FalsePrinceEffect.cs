using System.Collections;
using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields.Effects
{
    public class FalsePrinceEffect : Effect
    {
        private readonly Person _activePerson;

        public FalsePrinceEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Лжепринца закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            CallbackHandler callbackHandler = new CallbackHandler();

            _activePerson.ActivateFalsePrinceEffect(callbackHandler);
            yield return new WaitUntil(() => callbackHandler.IsComplete);
        }
    }
}