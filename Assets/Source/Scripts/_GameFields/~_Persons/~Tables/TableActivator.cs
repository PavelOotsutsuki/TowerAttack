using Tools;

namespace GameFields.Persons.Tables
{
    public class TableActivator : IWorkable
    {
        private readonly ITableActivator _tableActivator;

        public TableActivator(ITableActivator tableActivator)
        {
            _tableActivator = tableActivator;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _tableActivator.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _tableActivator.Deactivate();
        }
    }
}