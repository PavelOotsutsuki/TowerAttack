namespace GameFields.Persons.DrawCards
{
    public class DrawCardAnimationManager : IDrawCardAnimationWatcher, IFireDrawCardAnimationSetter
    {
        private readonly SimpleDrawCardAnimation _simpleDrawCardAnimation;
        private readonly FireDrawCardAnimation _fireDrawCardAnimation;

        private IDrawCardAnimation _currentAnimation;

        public DrawCardAnimationManager(SimpleDrawCardAnimation simpleDrawCardAnimation, FireDrawCardAnimation fireDrawCardAnimation)
        {
            _simpleDrawCardAnimation = simpleDrawCardAnimation;
            _fireDrawCardAnimation = fireDrawCardAnimation;

            SetSimpleMode();
        }

        public IDrawCardAnimation CurrentAnimation => _currentAnimation;

        public void SetFireMode()
        {
            _currentAnimation = _fireDrawCardAnimation;
        }

        public void SetSimpleMode()
        {
            _currentAnimation = _simpleDrawCardAnimation;
        }
    }
}