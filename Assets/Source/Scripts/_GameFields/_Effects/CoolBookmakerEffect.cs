using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class CoolBookmakerEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        private bool _endPlaying;

        public CoolBookmakerEffect(Person activePerson) : base()
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Четкого букмекера закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            _activePerson.ChoiceActivate(CountNumbers, EndPlayingCallback);
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