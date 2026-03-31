using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace StartMenues
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Init()
        {
            gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1f, 1f);
        }
    }
}
