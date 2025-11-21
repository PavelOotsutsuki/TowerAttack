using UnityEngine;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.DrawCards;
using System;

namespace GameFields.Effects
{
    public abstract class OgreVariantEffect : Effect
    {
        private readonly int _countDrawCards;
        private readonly int _countAttack;

        private readonly IDrawCardManager _drawCardManager;
        private readonly Person _attackPerson;

        private readonly Func<IEnumerator> _firstCoroutine;
        private readonly Func<IEnumerator> _secondCoroutine;

        public OgreVariantEffect(Person drawPerson, Person attackPerson, int countDrawCards,
            int countAttack, bool isFirstDraw, EffectData data) : base(data)
        {
            _drawCardManager = drawPerson;
            _attackPerson = attackPerson;

            _countDrawCards = countDrawCards;
            _countAttack = countAttack;

            if (isFirstDraw)
            {
                _firstCoroutine = Drawing;
                _secondCoroutine = Attacking;
            }
            else
            {
                _firstCoroutine = Attacking;
                _secondCoroutine = Drawing;
            }

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            yield return _firstCoroutine;
            yield return _secondCoroutine;
        }

        private IEnumerator Drawing()
        {
            bool isEndDraw = false;

            _drawCardManager.DrawCards(_countDrawCards, () => isEndDraw = true);

            yield return new WaitUntil(() => isEndDraw);
        }

        private IEnumerator Attacking()
        {
            bool isEndAttack = false;

            _attackPerson.AttackActivate(_countAttack, () => isEndAttack = true);

            yield return new WaitUntil(() => isEndAttack);
        }
    }
}