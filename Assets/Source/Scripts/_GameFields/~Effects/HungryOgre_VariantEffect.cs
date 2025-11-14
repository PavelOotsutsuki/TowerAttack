using UnityEngine;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.DrawCards;

namespace GameFields.Effects
{
    public abstract class HungryOgre_VariantEffect : Effect
    {
        private readonly int _countDrawCards;
        private readonly int _countAttack;

        private readonly IDrawCardManager _drawCardManager;
        private readonly Person _deactivePerson;

        private bool _isContinue;

        public HungryOgre_VariantEffect(Person activePerson, Person deactivePerson, int countDrawCards,
            int countAttack, EffectData data) : base(data)
        {
            _drawCardManager = activePerson;
            _deactivePerson = deactivePerson;

            _countDrawCards = countDrawCards;
            _countAttack = countAttack;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            _isContinue = false;

            _drawCardManager?.DrawCards(_countDrawCards, Continue);

            yield return new WaitUntil(() => _isContinue);
        }

        private void Continue()
        {
            _deactivePerson.AttackActivate(_countAttack, EndPlayingCallback);
        }

        private void EndPlayingCallback()
        {
            _isContinue = true;
        }
    }
}