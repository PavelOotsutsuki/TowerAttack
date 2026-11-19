namespace Cards.Views
{
    public class TagValuePair
    {
        private readonly string _tag;
        private readonly int _value;

        public TagValuePair(string tag, int value)
        {
            _tag = tag;
            _value = value;
        }

        public string Tag => _tag;
        public int Value => _value;
    }
}