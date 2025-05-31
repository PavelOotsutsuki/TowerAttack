using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public abstract class ButtonImageSpriteAndPressedColorChanger : ButtonImageChangerRealization
    {
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color PressedColor;

        [SerializeField] protected Sprite NormalSprite;
        [SerializeField] protected Sprite PressedSprite;
        [SerializeField] protected Sprite ClickSprite;
        [SerializeField] protected Sprite SelectSprite;

        protected Sprite CurrentSprite;

        public override void OnActivate()
        {
            Image.color = NormalColor;
            Image.sprite = NormalSprite;

            CurrentSprite = Image.sprite;
        }

        //public override void OnEnterClick()
        //{
        //    Image.color = _normalColor;
        //    Image.sprite = _clickSprite;
        //}

        //public override void OnExitClick()
        //{
        //    Image.color = _normalColor;
        //    Image.sprite = _normalSprite;
        //}

        //public override void OnPointerClick()
        //{
        //    _currentSprite = Image.sprite;
        //}

        public override void OnPointerDown()
        {
            Image.color = PressedColor;
            Image.sprite = SelectSprite;
        }

        public override void OnPointerEnter()
        {
            Image.color = NormalColor;
            Image.sprite = SelectSprite;
        }

        public override void OnPointerExit()
        {
            Image.color = NormalColor;
            Image.sprite = CurrentSprite;
        }

        public override void OnPointerUp()
        {
            Image.color = NormalColor;
            Image.sprite = CurrentSprite;
        }
    }
}