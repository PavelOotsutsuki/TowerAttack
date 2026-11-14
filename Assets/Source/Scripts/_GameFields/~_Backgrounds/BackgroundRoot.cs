using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Backgrounds
{
    public class BackgroundRoot : MonoBehaviour
    {
        [SerializeField] private BackgroundLogic1 _backgroundLogic1;
        [SerializeField] private Image _image1; 
        [SerializeField] private Image _image2;

        public void Init(CancellationToken fightToken)
        {
            SmoothlyImageChanger smoothlyImageChanger = new SmoothlyImageChanger(_image1, _image2);

            _backgroundLogic1.Init(smoothlyImageChanger, fightToken);
        }

        public void Activate()
        {
            _backgroundLogic1.Activate();
        }
    }
}