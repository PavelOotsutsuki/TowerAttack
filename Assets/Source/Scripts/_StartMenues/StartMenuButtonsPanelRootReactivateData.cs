using System;
using Tools;

namespace StartMenues
{
    public class StartMenuButtonsPanelRootReactivateData : IData, IActivatable
    {
        private readonly Action _onReactivate;

        public StartMenuButtonsPanelRootReactivateData(Action onReactivate)
        {
            _onReactivate = onReactivate;
        }

        public void Activate()
        {
            _onReactivate?.Invoke();
        }
    }
}