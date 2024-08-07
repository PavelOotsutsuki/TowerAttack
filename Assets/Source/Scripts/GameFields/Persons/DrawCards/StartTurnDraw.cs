using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class StartTurnDraw : ITurnStep
    {
        private readonly int _countDrawCards;
        private readonly CardDrawer _cardDrawer;
        
        public bool IsComplete { get; private set; }

        public StartTurnDraw(CardDrawer cardDrawer, int countDrawCards)
        {
            _cardDrawer = cardDrawer;
            _countDrawCards = countDrawCards;
        }

        public void StartStep()
        {
            IsComplete = false;
            DrawingCards().ToUniTask();

            _cardDrawer.Update();
        }

        private IEnumerator DrawingCards()
        {
            _cardDrawer.DrawCards(_countDrawCards);

            yield return new WaitUntil(() => _cardDrawer.IsDrawing == false);

            IsComplete = true;
        }
    }

    public class CardDrawer
    {
        private readonly DrawCardRoot _drawCardRoot;
        private readonly IDrawCardAnimation _simpleDrawCardAnimation;
        private readonly IDrawCardAnimation _fireDrawCardAnimation;
        
        private int _countExtraAnimationTurns;
        private IDrawCardAnimation _currentAnimation;

        public CardDrawer(DrawCardRoot drawCardRoot, IDrawCardAnimation simpleDrawCardAnimation, IDrawCardAnimation fireDrawCardAnimation)
        {
            _drawCardRoot = drawCardRoot;
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
            _fireDrawCardAnimation = fireDrawCardAnimation;
        }
        
        public bool IsDrawing => _drawCardRoot.IsDrawing;

        public void SetSimpleMode()
        {
            _currentAnimation = _simpleDrawCardAnimation;
        }

        public void SetFireMode(int countTurns)
        {
            _currentAnimation = _fireDrawCardAnimation;
            _countExtraAnimationTurns = countTurns;
        }

        public void DrawCards(int count)
        {
            _drawCardRoot.DrawCards(_currentAnimation, count);
        }

        public void Update()
        {
            if (_countExtraAnimationTurns > 0)
            {
                _countExtraAnimationTurns--;

                if (_countExtraAnimationTurns <= 0)
                {
                    SetSimpleMode();
                }
            }
        }
    }

    public class SimpleCardDrawer
    {
        private readonly DrawCardRoot _drawCardRoot;
        private readonly IDrawCardAnimation _simpleDrawCardAnimation;

        public SimpleCardDrawer(DrawCardRoot drawCardRoot, IDrawCardAnimation simpleDrawCardAnimation)
        {
            _drawCardRoot = drawCardRoot;
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
        }
        
        public void DrawCards(int count, System.Action callback = null)
        {
            _drawCardRoot.DrawCards(_simpleDrawCardAnimation, count, callback);
        }
    }
}
