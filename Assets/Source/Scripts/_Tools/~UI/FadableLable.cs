using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadablePanel))]
    [RequireComponent(typeof(Label))]
    public class FadableLabel : MonoBehaviour, ICompletable, IViewable, IShowable<LabelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private Label _label;
        [SerializeField] private FadablePanel _fadablePanel;

        public bool IsComplete => _fadablePanel.IsComplete;
        public bool? IsShown => _fadablePanel.IsShown;

        //public int TextLength => _label.TextLength;

        public virtual void Init()
        {
            _label.Init();
            _fadablePanel.Init();
        }

        public void Show(LabelActivateData data)
        {
            _label.SetText(data.Message);

            Show();
        }

        public void Show()
        {
            _fadablePanel.Show();
        }

        public virtual void Hide()
        {
            _fadablePanel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadableLabel))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLabel(),
                DefineFadablePanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLabel))]
        private ComponentAttachInfo DefineLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}