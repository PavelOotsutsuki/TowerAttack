using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFields
{
    public class SpeedUpButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private const float DefaultSpeed = 1f;

        [SerializeField] private Image _image;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _activeSpeedColor;
        [SerializeField] private float _activeSpeed = 2f;

        private Color _currentColor;

        public void OnEnable()
        {
            SetNormalSettings();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Mathf.Approximately(Time.timeScale, DefaultSpeed))
            {
                SetActiveSpeedSettings();
            }
            else
            {
                SetNormalSettings();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = _selectColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _currentColor;
        }

        private void SetNormalSettings()
        {
            Time.timeScale = DefaultSpeed;

            _image.color = _normalColor;

            _currentColor = _image.color;
        }

        private void SetActiveSpeedSettings()
        {
            Time.timeScale = _activeSpeed;

            _image.color = _activeSpeedColor;

            _currentColor = _image.color;
        }
    }
}
