namespace GameFields.EndTurnButtons
{
    public interface IButtonActivator
    {
        public bool IsActive { get; }

        public void SetActiveSide();
    }
}
