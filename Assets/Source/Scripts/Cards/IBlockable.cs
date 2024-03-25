namespace Cards
{
    public interface IBlockable
    {
        public bool IsBlocked { get; }

        public void Block();
        public void Unblock();
    }
}