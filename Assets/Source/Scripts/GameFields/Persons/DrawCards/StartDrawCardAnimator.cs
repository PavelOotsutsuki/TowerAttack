using Cards;

namespace GameFields.Persons.DrawCards
{
    public class StartDrawCardAnimator : IDrawCardAnimation
    {
        private SimpleDrawCardAnimation _simpleDrawCardAnimation;
        private FireDrawCardAnimation _fireDrawCardAnimation;
        private int _countExtraAnimationTurns;

        private IDrawCardAnimation _currentAnimation;

        public bool IsDone => _currentAnimation.IsDone;

        public StartDrawCardAnimator(SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation)
        {
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
            _fireDrawCardAnimation = fireDrawCardAnimation;

            _currentAnimation = _simpleDrawCardAnimation;
            _countExtraAnimationTurns = 0;
        }

        public void SetFireMode(int countTurns)
        {
            _currentAnimation = _fireDrawCardAnimation;

            _countExtraAnimationTurns = countTurns;
        }

        public void Play(Card card)
        {
            _currentAnimation.Play(card);

            if (_countExtraAnimationTurns > 0)
            {
                _countExtraAnimationTurns--;

                if (_countExtraAnimationTurns == 0)
                {
                    SetSimpleMode();
                }
            }
        }

        private void SetSimpleMode()
        {
            _currentAnimation = _simpleDrawCardAnimation;
        }
    }
}
