using UnityEngine;
using UnityEngine.UI;

namespace Tools.UI.ImageChangers.V1
{
    public abstract class ButtonImageChangerRealization : ButtonImageChanger
    {
        [SerializeField] protected Image Image;
    }
}