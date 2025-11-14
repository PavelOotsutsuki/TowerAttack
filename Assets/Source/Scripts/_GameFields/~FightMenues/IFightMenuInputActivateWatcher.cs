namespace GameFields.FightMenues
{
    public interface IFightMenuInputActivateWatcher
    {
        public IFocusedButtonEnterHandler CurrentFightMenuButtonInputHandler { get; }
    }
}