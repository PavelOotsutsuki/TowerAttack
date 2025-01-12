using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.LightControls
{
    [RequireComponent(typeof(FadablePanel))]
    public class LightPanel : MonoBehaviour, IViewable, IAutomaticFillComponents//FadablePanel
    {
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private Transform _transform;

        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _fadablePanel.Init();
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            _fadablePanel.Show();
            _transform.SetAsFirstSibling();
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;
            _fadablePanel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LightPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel(),
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}