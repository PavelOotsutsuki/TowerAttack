using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameFields.LightControls
{
    public abstract class LightController : IWorkable
    {
        private readonly IViewable _lightPanel;

        public LightController(LightPanel lightPanel)
        {
            _lightPanel = lightPanel;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _lightPanel.Show();

            OnActivate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _lightPanel.Hide();

            OnDeactivate();
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}
