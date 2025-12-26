using Cards.DependencyInterlayers;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZonePlayer : CardPlayingZone, ITableDrop, ITableActivator
    {
        private bool _canPlay = false;

        public bool CanPlay => _canPlay;

        void ITableActivator.Activate()
        {
            _canPlay = true;
        }

        void ITableActivator.Deactivate()
        {
            _canPlay = false;
        }
    }
}