using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
    internal class ScreenRoot : MonoBehaviour
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
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAllCanvasScalers();
        }

        [ContextMenu(nameof(DefineAllCanvasScalers))]
        private void DefineAllCanvasScalers()
        {
            AutomaticFillComponents.DefineComponent(this, ref _allCanvasScalers);
        }
        #endregion
    }
}
