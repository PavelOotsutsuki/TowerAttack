namespace Tools.UI.UIHelpers
{
    public class UIHelperDescriptionActivateData : IData
    {
        private readonly string _text;
        private readonly ReadOnlyRectTransform _logicChildTransform;

        public UIHelperDescriptionActivateData(string text, ReadOnlyRectTransform logicChildTransform)
        {
            _text = text;
            _logicChildTransform = logicChildTransform;
        }

        public LabelActivateData LabelActivateData => new LabelActivateData(_text);
        public ReadOnlyRectTransform LogicChildTransform => _logicChildTransform;
    }
}