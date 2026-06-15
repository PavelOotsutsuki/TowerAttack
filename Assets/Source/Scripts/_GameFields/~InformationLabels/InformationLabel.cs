using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.InformationLabels
{
    public class InformationLabel : MonoBehaviour, IActivatable<InformationLabelActivateData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private InformationLabelLabel _informationLabel;
        [SerializeField] private InformationLabelPanel _panel;

        private CancellationToken _fightToken;
        private bool _isComplete;
        private bool _isActive;
        private InformationLabelActivateData _currentData;

        private CancellationTokenSource _currentCTS;

        public bool IsComplete => _isComplete && _informationLabel.IsComplete && _panel.IsComplete;

        public void Init(CancellationToken fightToken)
        {
            _fightToken = fightToken;

            _informationLabel.Init();
            _panel.Init();

            _currentData = null;
            gameObject.SetActive(false);
        }

        public void Activate(InformationLabelActivateData data)
        {
            if (_currentData != null)
            {
                _currentData += data;
            }
            else
            {
                _currentData = data;
            }

            _isComplete = false;

            gameObject.SetActive(true);

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _informationLabel.Show(new LabelActivateDataAsync(_currentData.LabelActivateData, _currentCTS.Token));
            _panel.Show(new CancellationTokenData(_currentCTS.Token));

            WaitUntilDeactivating(data.TimeView, _currentCTS.Token).Forget();
        }

        private void Deactivate(CancellationToken token)
        {
            _informationLabel.Hide(new CancellationTokenData(token));
            _panel.Hide(new CancellationTokenData(token));

            Deactivating(token).Forget();
        }

        private async UniTask WaitUntilDeactivating(float timeView, CancellationToken token)
        {
            await UniTask.WaitUntil(() => _informationLabel.IsComplete && _panel.IsComplete, cancellationToken: token);
            await UniTask.WaitForSeconds(timeView / 2f, cancellationToken: token);
            GameFieldGC.Collect();
            await UniTask.WaitForSeconds(timeView / 2f, cancellationToken: token);

            Deactivate(token);
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _informationLabel.IsComplete && _panel.IsComplete, cancellationToken: token);

            _currentData = null;
            Utils.DestroyCTS(ref _currentCTS);
            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(InformationLabel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineInformationLabel(),
                DefineInformationLabelPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineInformationLabel))]
        private ComponentAttachInfo DefineInformationLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _informationLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineInformationLabelPanel))]
        private ComponentAttachInfo DefineInformationLabelPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _panel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}