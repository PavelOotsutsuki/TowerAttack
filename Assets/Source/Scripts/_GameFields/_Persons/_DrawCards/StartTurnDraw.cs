using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public abstract class StartTurnDraw : PersonStep
    {
        private readonly int _countDrawCards;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly SimpleDrawCardAnimation _simpleDrawCardAnimation;
        private readonly FireDrawCardAnimation _fireDrawCardAnimation;

        private bool _isComplete;
        private int _countExtraAnimationTurns;

        private IDrawCardAnimation _currentAnimation;

        public StartTurnDraw(GameFieldObjectsActivator gameFieldObjectsActivator, DrawCardRoot drawCardRoot,
            SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation,
            int countDrawCards) :base(gameFieldObjectsActivator)
        {
            _drawCardRoot = drawCardRoot;
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
            _fireDrawCardAnimation = fireDrawCardAnimation;
            _countDrawCards = countDrawCards;

            _currentAnimation = _simpleDrawCardAnimation;
            _countExtraAnimationTurns = 0;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;
            DrawingCards().ToUniTask();

            if (_countExtraAnimationTurns > 0)
            {
                _countExtraAnimationTurns--;

                if (_countExtraAnimationTurns <= 0)
                {
                    SetSimpleMode();
                }
            }
        }

        public void SetFireMode(int countTurns)
        {
            _currentAnimation = _fireDrawCardAnimation;

            _countExtraAnimationTurns = countTurns;
        }

        private IEnumerator DrawingCards()
        {
            _drawCardRoot.DrawCards(_currentAnimation, _countDrawCards);

            yield return new WaitUntil(() => _drawCardRoot.IsDrawing == false);

            _isComplete = true;
        }

        private void SetSimpleMode()
        {
            _currentAnimation = _simpleDrawCardAnimation;
        }
    }
}