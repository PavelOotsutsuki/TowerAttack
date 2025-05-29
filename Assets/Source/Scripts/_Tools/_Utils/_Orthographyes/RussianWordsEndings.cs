namespace Tools.Utils.Orthographyes
{
    internal struct RussianWordsEndings
    {
        public string OneEnding { get; private set; }
        public string TwoThreeFourEnding { get; private set; }
        public string DefaultEnding { get; private set; }

        public RussianWordsEndings(string oneEnding, string twoThreeFourEnding, string defaultEnding)
        {
            OneEnding = oneEnding;
            TwoThreeFourEnding = twoThreeFourEnding;
            DefaultEnding = defaultEnding;
        }
    }
}