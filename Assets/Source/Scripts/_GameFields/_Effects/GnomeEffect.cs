using System.Collections;
using Cards;
using GameFields.Persons.Common;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public abstract class GnomeEffect : Effect
    {
        private readonly Person _activePerson;

        private bool _endPlaying;

        public GnomeEffect(Person activePerson) : base()
        {
            _activePerson = activePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            if (_activePerson.PersonEffectsCounter.GnomeEffectCounter.TryActivate(out int countNumbers))
            {
                _activePerson.ChoiceActivate(countNumbers, EndPlaying);
            }
            else
            {
                EndPlaying();
            }
            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);
            yield return new WaitUntil(() => _endPlaying);

            //_deactivePerson.AttackDeactivate();
        }

        private void EndPlaying()
        {
            _endPlaying = true;
        }
    }
}