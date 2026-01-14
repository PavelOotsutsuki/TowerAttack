using UnityEngine;

namespace Tools.UI.ImageChangers
{
    public abstract class ButtonImageSpriteAndPressedColorChanger : ButtonImageChanger, IButtonImageChanger
    {
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color PressedColor;

        [SerializeField] protected Sprite NormalSprite;
        [SerializeField] protected Sprite PressedSprite;
        [SerializeField] protected Sprite ClickSprite;
        [SerializeField] protected Sprite SelectSprite;

        protected Sprite CurrentSprite;

        public void OnActivate()
        {
            Image.color = NormalColor;
            Image.sprite = NormalSprite;

            CurrentSprite = Image.sprite;
        }

        public void OnPointerDown()
        {
            Image.color = PressedColor;
            Image.sprite = SelectSprite;
        }

        public void OnPointerEnter()
        {
            Image.color = NormalColor;
            Image.sprite = SelectSprite;
        }

        public void OnPointerExit()
        {
            Image.color = NormalColor;
            Image.sprite = CurrentSprite;
        }

        public void OnPointerUp()
        {
            Image.color = NormalColor;
            Image.sprite = CurrentSprite;
        }
    }
}