using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDraw : ITurnStep
    {
        private int _countDrawCards;

        private DrawCardRoot _drawCardRoot;
        private SimpleDrawCardAnimation _simpleDrawCardAnimation;
        private FireDrawCardAnimation _fireDrawCardAnimation;
        private bool _isComplete;
        private int _countExtraAnimationTurns;

        private IDrawCardAnimation _currentAnimation;

        public bool IsComplete => _isComplete;

        public StartTurnDraw(DrawCardRoot drawCardRoot, SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation, int countDrawCards)
        {
            _drawCardRoot = drawCardRoot;
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
            _fireDrawCardAnimation = fireDrawCardAnimation;
            _countDrawCards = countDrawCards;

            _currentAnimation = _simpleDrawCardAnimation;
            _countExtraAnimationTurns = 0;
        }

        //public void PrepareToStart()
        //{
        //    _isComplete = false;
        //}

        public void StartStep()
        {
            DrawingCards().ToUniTask();

            if (_countExtraAnimationTurns > 0)
            {
                _countExtraAnimationTurns--;

                if (_countExtraAnimationTurns == 0)
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
