using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields
{
    public class SpeedUpButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnClick()
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 2;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
