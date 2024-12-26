using System.Collections.Generic;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
    internal class ScreenRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private CanvasScaler[] _allCanvasScalers;
        [SerializeField] private Vector2 _defaultReferenceResolution = new Vector2(1920f, 1080f);

        public void Init()
        {
            DefineReferenceResolution();
        }

        private void DefineReferenceResolution()
        {
            foreach (CanvasScaler canvasScaler in _allCanvasScalers)
            {
                canvasScaler.referenceResolution = _defaultReferenceResolution;
            }

            ScreenView.SetReferenceResolution(_defaultReferenceResolution);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ScreenRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAllCanvasScalers()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAllCanvasScalers))]
        private ComponentAttachInfo DefineAllCanvasScalers()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _allCanvasScalers);
        }
        #endregion
    }
}