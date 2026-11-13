namespace Tools.InputSettings
{
    public class NextFocusData : IData
    {
        private readonly object _onDown;
        private readonly object _onUp;
        private readonly object _onLeft;
        private readonly object _onRight;

        public NextFocusData(object onDown, object onUp, object onLeft, object onRight)
        {
            _onDown = onDown;
            _onUp = onUp;
            _onLeft = onLeft;
            _onRight = onRight;
        }

        public object GetSide(InputSideType inputSideType)
        {
            switch (inputSideType)
            {
                case InputSideType.OnDown:
                    return _onDown;
                case InputSideType.OnUp:
                    return _onUp;
                case InputSideType.OnLeft:
                    return _onLeft;
                case InputSideType.OnRight:
                    return _onRight;
                default:
                    throw new System.Exception("Неизвестный inputSideType: " + inputSideType.ToString());
            }
        }
    }
}