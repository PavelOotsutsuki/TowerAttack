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
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            _activePerson.ChoiceActivate(CountNumbers);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);
            yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}