using System.Collections.Generic;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.Utils.Screens
{
    public class ScreenRoot : MonoBehaviour, IResolutionSetter, ICurrentResolutionInfo, IResolutionInfo, IAutomaticFillComponents
    {
        [SerializeField] private CanvasScaler[] _allCanvasScalers;

        private readonly Dictionary<ResolutionType, ResolutionTypeData> _resolutionsData = new Dictionary<ResolutionType, ResolutionTypeData>();

        private ResolutionType _currentResolution;

        public void Init()
        {
            _resolutionsData.Add(ResolutionType._960x540, new ResolutionTypeData("960x540", new Vector2(960f, 540f)));
            _resolutionsData.Add(ResolutionType._1920x1080, new ResolutionTypeData("1920x1080", new Vector2(1920f, 1080f)));
            _resolutionsData.Add(ResolutionType._3840x2160, new ResolutionTypeData("3840x2160", new Vector2(3840f, 2160f)));

            Resolution[] resolutions = Screen.resolutions;

            foreach (Resolution resolution in resolutions)
            {
                Debug.Log($"resolution.height = {resolution.height}, resolution.width = {resolution.width}");
            }

            Debug.Log(Screen.currentResolution);

            SetResolution(GameSettings.DefaultResolutionType);
        }

        public ResolutionType Type => _currentResolution;
        public string Text => _resolutionsData[_currentResolution].Text;
        public Vector2 Vector => _resolutionsData[_currentResolution].Vector2;
        public float FactorX => Screen.width / Vector.x;
        public float FactorY => Screen.height / Vector.y;
        public float X => Vector.x;
        public float Y => Vector.y;

        public void SetResolution(ResolutionType resolutionType)
        {
            _currentResolution = resolutionType;

            Vector2 referenceResolution = Vector;

            foreach (CanvasScaler canvasScaler in _allCanvasScalers)
            {
                canvasScaler.referenceResolution = referenceResolution;
            }
        }

        public ResolutionType GetResolutionType(string text)
        {
            foreach (KeyValuePair<ResolutionType, ResolutionTypeData> data in _resolutionsData)
            {
                if (data.Value.Text == text)
                    return data.Key;
            }

            throw new System.Exception($"Не найден ResolutionType по text: {text}");
        }

        public string GetResolutionText(ResolutionType resolutionType)
        {
            return _resolutionsData[resolutionType].Text;
        }

        public Vector2 GetResolutionVector(ResolutionType resolutionType)
        {
            return _resolutionsData[resolutionType].Vector2;
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
           return AutomaticFillComponents.DefineComponent(this, ref _allCanvasScalers, true);
        }
        #endregion
    }
}