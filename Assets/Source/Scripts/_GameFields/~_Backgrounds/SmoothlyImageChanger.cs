using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Backgrounds
{
    public class SmoothlyImageChanger
    {
        private readonly float _switchDuration = 4f;

        private Image _activeImage;
        private Image _deactiveImage;

        public SmoothlyImageChanger(Image image1, Image image2)
        {
            if (image1.gameObject.activeSelf)
            {
                _activeImage = image1;
                _deactiveImage = image2;
            }
            else
            {
                _activeImage = image2;
                _deactiveImage = image1;
            }

            _activeImage.gameObject.SetActive(true);
            _activeImage.color = new Color(_activeImage.color.r, _activeImage.color.g, _activeImage.color.b, 1f);
            _deactiveImage.gameObject.SetActive(false);
            _deactiveImage.color = new Color(_deactiveImage.color.r, _deactiveImage.color.g, _deactiveImage.color.b, 0f);
        }

        public void SetImageInstantly(Sprite sprite)
        {
            _activeImage.sprite = sprite;
        }

        public void SetImageSmoothly(Sprite sprite)
        {
            _deactiveImage.sprite = sprite;
            _deactiveImage.gameObject.SetActive(true);

            Sequence sequence = DOTween.Sequence()
                .Join(_activeImage.DOFade(0f, _switchDuration))
                .Join(_deactiveImage.DOFade(1f, _switchDuration))
                .OnComplete(() => SwitchImage());
        }

        //private async UniTask  

        private void SwitchImage()
        {
            Image activeImage = _activeImage;
            _activeImage = _deactiveImage;
            _deactiveImage = activeImage;
        }
    }
}