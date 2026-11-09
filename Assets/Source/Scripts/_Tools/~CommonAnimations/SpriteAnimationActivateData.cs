namespace Tools.CommonAnimations
{
    public class SpriteAnimationActivateData : IData
    {
        private readonly bool _isActiveView;

        public SpriteAnimationActivateData(bool isActiveView)
        {
            _isActiveView = isActiveView;
        }

        public bool IsActiveView => _isActiveView;
    }
}