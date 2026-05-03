using TMPro;
using UnityEngine.UI;

namespace Tools.UI
{
    public interface IFocusUnityButtonWatcher
    {
        public void SetFocused(TMP_InputField inputField);
    }
}