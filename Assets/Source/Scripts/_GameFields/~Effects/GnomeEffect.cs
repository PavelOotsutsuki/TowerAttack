using System.Collections;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public abstract class GnomeEffect : Effect
    {
        private readonly Person _activePerson;

        public GnomeEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Гнома закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            bool endGmoneSearch = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            if (_activePerson.TryActivateGnomeEffect(out int countNumbers))
            {
                _activePerson.ChoiceActivate(countNumbers, () => endGmoneSearch = true);
                yield return new WaitUntil(() => endGmoneSearch);
            }

            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);

            //_deactivePerson.AttackDeactivate();
        }
    }
}