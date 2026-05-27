using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.LightControls
{
    public class LightController : IWorkable<LightControllerActivateData>, IBlockable
    {
        private readonly LightPanel _lightPanel;
        private readonly WaitForSeconds _WFS_DelayForActivate;

        private IEnumerable<LightableObject> _currentLightableObjects;
        private CancellationTokenSource _token;
        private UniTask _activating;
        private bool _isActivatable;

        public LightController(LightPanel lightPanel, float delayForActivate)
        {
            _lightPanel = lightPanel;
            _WFS_DelayForActivate = new WaitForSeconds(delayForActivate);

            _isActivatable = true;
        }

        ~LightController()
        {
            _token.Dispose();
        }

        public bool? IsActive { get; private set; } = false;

        public void Activate(LightControllerActivateData data)
        {
            if (_isActivatable == false)
                return;

            if (IsActive == true)
                return;

            IsActive = true;
            _currentLightableObjects = data.LightableObjects;

            CancelActivating();

            _token = new CancellationTokenSource();
            _activating = Activating().ToUniTask(cancellationToken: _token.Token);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            CancelActivating();

            foreach (LightableObject lightableObject in _currentLightableObjects)
            {
                lightableObject.Hide();
            }

            _lightPanel.Hide();
        }

        public void Unblock()
        {
            _isActivatable = true;
        }

        public void Block()
        {
            _isActivatable = false;

            Deactivate();
        }

        private IEnumerator Activating()
        {
            yield return _WFS_DelayForActivate;

            foreach (LightableObject lightableObject in _currentLightableObjects)
            {
                lightableObject.Show();
            }

            _lightPanel.Show();
        }

        private void CancelActivating()
        {
            if (_activating.Status == UniTaskStatus.Pending)
            {
                _token.Cancel();
            }
        }
    }
}