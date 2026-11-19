using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    internal abstract class StartTurnDraw : PersonStep, IPersonObject
    {
        private readonly int _countDrawCards;
        private readonly DrawCardRoot _drawCardRoot;
        //private readonly SimpleDrawCardAnimation _simpleDrawCardAnimation;
        //private readonly FireDrawCardAnimation _fireDrawCardAnimation;

        private bool _isComplete;
        //private int _countExtraAnimationTurns;

        //private IDrawCardAnimation _currentAnimation;

        //public StartTurnDraw(InteractionActivator interactionActivator, DrawCardRoot drawCardRoot,
        //    SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation,
        //    int countDrawCards) :base(interactionActivator)
        public StartTurnDraw(InteractionActivator interactionActivator, DrawCardRoot drawCardRoot,
            int countDrawCards, CancellationToken fightToken) : base(interactionActivator, fightToken)
        {
            _drawCardRoot = drawCardRoot;
            //_simpleDrawCardAnimation = simpleDrawCardAnimation;
            //_fireDrawCardAnimation = fireDrawCardAnimation;
            _countDrawCards = countDrawCards;

            //_currentAnimation = _simpleDrawCardAnimation;
            //_countExtraAnimationTurns = 0;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            DrawingCards().Forget();

            //if (_countExtraAnimationTurns > 0)
            //{
            //    _countExtraAnimationTurns--;

            //    if (_countExtraAnimationTurns <= 0)
            //    {
            //        SetSimpleMode();
            //    }
            //}
        }

        //public void SetFireMode(int countTurns)
        //{
        //    _currentAnimation = _fireDrawCardAnimation;

        //    _countExtraAnimationTurns = countTurns;
        //}

        private async UniTask DrawingCards()
        {
            if (Token.IsCancellationRequested)
                return;

            try
            {
                _drawCardRoot.DrawCards(_countDrawCards, Token);

                await UniTask.WaitUntil(() => _drawCardRoot.IsDrawing == false, cancellationToken: Token);

                _isComplete = true;
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        //private void SetSimpleMode()
        //{
        //    _currentAnimation = _simpleDrawCardAnimation;
        //}
    }
}