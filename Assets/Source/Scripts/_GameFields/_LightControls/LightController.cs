using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.LightControls
{
    public class LightController : IWorkable, IBlockable
    {
        private const float DelayForActivate = 5f;

        private readonly LightPanel _lightPanel;
        private readonly LightableObject[] _lightableObjects;
        private readonly WaitForSeconds _WFS_DelayForActivate;

        private CancellationTokenSource _token;
        private UniTask _activating;
        private bool _isActivatable;

        public LightController(LightPanel lightPanel, LightableObject[] lightableObjects)
        {
            _lightPanel = lightPanel;
            _lightableObjects = lightableObjects;
            _WFS_DelayForActivate = new WaitForSeconds(DelayForActivate);

            _isActivatable = true;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (_isActivatable == false)
                return;

            if (IsActive == true)
                return;

            IsActive = true;

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

            foreach (LightableObject lightableObject in _lightableObjects)
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

            foreach (LightableObject lightableObject in _lightableObjects)
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