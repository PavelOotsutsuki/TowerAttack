using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class CoolBookmakerEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        private bool _endPlaying;

        public CoolBookmakerEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Четкого букмекера закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            _activePerson.ChoiceActivate(CountNumbers, EndPlayingCallback, RestrictionType.Even);
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);
            yield return new WaitUntil(() => _endPlaying);

            //_deactivePerson.AttackDeactivate();
        }

        private void EndPlayingCallback()
        {
            _endPlaying = true;
        }
    }
}