public struct CardSize
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    public CardSize(float width, float height)
    {
        Width = width;
        Height = height;
    }
}
