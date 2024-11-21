namespace Cards
{
    internal struct CardSize
    {
        internal float Width { get; private set; }
        internal float Height { get; private set; }

        internal CardSize(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}

