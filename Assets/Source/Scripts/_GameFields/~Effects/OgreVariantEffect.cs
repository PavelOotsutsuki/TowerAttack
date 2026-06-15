using GameFields.Persons;
using GameFields.Persons.DrawCards;
using System;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public abstract class OgreVariantEffect : Effect
    {
        private readonly int _countDrawCards;
        private readonly int _countAttack;

        private readonly IDrawCardManager _drawCardManager;
        private readonly Person _attackPerson;

        private readonly Func<UniTask> _firstCoroutine;
        private readonly Func<UniTask> _secondCoroutine;

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

        protected override async UniTask OnPlaying()
        {
            await _firstCoroutine.Invoke();
            await _secondCoroutine.Invoke();
        }

        private async UniTask Drawing()
        {
            bool isEndDraw = false;

            _drawCardManager.DrawCards(_countDrawCards, Token, () => isEndDraw = true);

            await UniTask.WaitUntil(() => isEndDraw, cancellationToken: Token);
        }

        private async UniTask Attacking()
        {
            bool isEndAttack = false;

            _attackPerson.AttackActivate(_countAttack, () => isEndAttack = true);

            await UniTask.WaitUntil(() => isEndAttack, cancellationToken: Token);
        }
    }
}