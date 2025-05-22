using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.InformationLabels
{
    public class InformationLabel : MonoBehaviour, IActivatable<InformationLabelActivateData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private InformationLabelLabel _informationLabel;
        [SerializeField] private InformationLabelPanel _panel;

        private bool _isComplete;

        public bool IsComplete => _isComplete && _informationLabel.IsComplete && _panel.IsComplete;

        public void Init()
        {
            _informationLabel.Init();
            _panel.Init();

            gameObject.SetActive(false);
        }

        public void Activate(InformationLabelActivateData data)
        {
            _isComplete = false;

            gameObject.SetActive(true);

            _informationLabel.Show(data.LabelActivateData);
            _panel.Show();

            StartCoroutine(WaitUntilDeactivating(data.TimeView));
        }

        private void Deactivate()
        {
            _informationLabel.Hide();
            _panel.Hide();

            StartCoroutine(Deactivating());
        }

        private IEnumerator WaitUntilDeactivating(float timeView)
        {
            yield return new WaitUntil(() => _informationLabel.IsComplete && _panel.IsComplete);
            yield return new WaitForSeconds(timeView / 2f);
            GC.Collect();
            yield return new WaitForSeconds(timeView / 2f);

            Deactivate();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _informationLabel.IsComplete && _panel.IsComplete);

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