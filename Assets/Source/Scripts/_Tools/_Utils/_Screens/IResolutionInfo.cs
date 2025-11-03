using UnityEngine;

namespace Tools.Utils.Screens
{
    public interface IResolutionInfo
    {
        public ResolutionType GetResolutionType(string text);
        public string GetResolutionText(ResolutionType resolutionType);
        public Vector2 GetResolutionVector(ResolutionType resolutionType);
    }
}