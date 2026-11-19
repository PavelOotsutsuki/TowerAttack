namespace Cards.Views
{
    internal class DefaultTagValuePair
    {
        private readonly string _tag;
        private readonly string _value;

        public DefaultTagValuePair(string tag, string value)
        {
            _tag = tag;
            _value = value;
        }

        public string Tag => _tag;
        public string Value => _value;
    }
}