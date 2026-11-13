using System.Collections.Generic;
using System.Linq;
using TMPro;
using Tools.Settings;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
    public class CanvasRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private CanvasScaler[] _allCanvasScalers;
        //[SerializeField] private Vector2 _defaultReferenceResolution = new Vector2(1920f, 1080f);

        public void Init()
        {
            SetReferenceResolution(_allCanvasScalers);
        }

        private void SetReferenceResolution(IEnumerable<CanvasScaler> canvasScalers)
        {
            if (canvasScalers != null)
                if (canvasScalers.Count() > 0)
                    foreach (CanvasScaler canvasScaler in canvasScalers)
                    {
                        canvasScaler.referenceResolution = GameSettings.CanvasReferenceResolution;
                    }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CanvasRoot))]
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
            return AutomaticFillComponents.DefineComponent(this, ref _allCanvasScalers, true);
        }
        #endregion
    }
}