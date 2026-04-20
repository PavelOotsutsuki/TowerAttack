using UnityEngine;
using UnityEngine.UI;
using System;

namespace Cards.Insides
{
    [Serializable]
    public class CardBlock
    {
        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;
        [SerializeField] private CanvasGroup _canvasGroup;

        public bool IsBlock => _canvasGroup.blocksRaycasts == false;

        public void Block()
        {
            _frameImage.color = _disableFrameColor;

            _canvasGroup.blocksRaycasts = false;
        }

        public void Unblock()
        {
            _frameImage.color = _enableFrameColor;

            _canvasGroup.blocksRaycasts = true;
        }
    }
}