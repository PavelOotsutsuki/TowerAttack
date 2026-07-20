using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils;

namespace GameFields.LightControls
{
    public class LightController : IWorkable<LightControllerActivateData>, IBlockable
    {
        private readonly LightPanel _lightPanel;
        private readonly float _delayForActivate;
        private readonly CancellationToken _fightToken;

        private IEnumerable<LightableObject> _currentLightableObjects;
        private CancellationTokenSource _currentCTS;
        //private UniTask _activating;
        private bool _isActivatable;

        public LightController(LightPanel lightPanel, float delayForActivate, CancellationToken fightToken)
        {
            _lightPanel = lightPanel;
            _delayForActivate = delayForActivate;
            _fightToken = fightToken;

            _currentCTS = null;
            _isActivatable = true;
        }

        //~LightController()
        //{
        //    _currentCTS.Dispose();
        //}

        public bool? IsActive { get; private set; } = false;

        public void Activate(LightControllerActivateData data)
        {
            if (_isActivatable == false)
                return;

            if (IsActive == true)
                return;

            IsActive = true;
            _currentLightableObjects = data.LightableObjects;

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            Activating(_currentCTS.Token).Forget();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            Utils.DestroyCTS(ref _currentCTS);

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

        private async UniTask Activating(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_delayForActivate, cancellationToken: token);

            foreach (LightableObject lightableObject in _currentLightableObjects)
            {
                lightableObject.Show();
            }

            _lightPanel.Show();
        }

        //private void CancelActivating()
        //{
        //    if (_activating.Status == UniTaskStatus.Pending)
        //    {
        //        _currentCTS.Cancel();
        //    }
        //}
    }
}