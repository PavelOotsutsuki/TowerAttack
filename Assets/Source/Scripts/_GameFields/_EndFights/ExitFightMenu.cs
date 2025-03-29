using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.EndFights
{
    public class ExitFightMenu : MonoBehaviour, IActivatable, IPointerClickHandler
    {
        public void Init()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.Quit();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}