using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class CardFront : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private float _width = 150f;
        [SerializeField] private float _height = 210f;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;
        [SerializeField] private CanvasGroup _canvasGroup;

        private RectTransform _cardRectTransform;
        private CardDescription _cardDescription;
        private BigCard _bigCard;
        private CardSize _cardSize;
        private CardSO _cardSO;
        private bool _isBlock;

        internal void Init(CardSO cardSO, RectTransform cartRectTransform, CardDescription cardDescription, BigCard bigCard)
        {
            _isBlock = false;
            _cardSize = new CardSize(_width, _height);
            _cardRectTransform = cartRectTransform;
            _cardDescription = cardDescription;
            _bigCard = bigCard;
            _cardSO = cardSO;

            DefineViewCharacters();
            DefineSmallSize();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isBlock)
            {
                return;
            }

            _cardDescription.Show(_cardSO.Description);
            DefineBigCard();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isBlock)
            {
                return;
            }

            _cardDescription.Hide();
            DefineSmallCard();
        }

        internal void TranslateInto(Vector2 positon, Vector3 rotation, float duration)
        {
            _cardRectTransform.DOMove(positon, duration);
            _cardRectTransform.DORotate(rotation, duration);
        }

        internal void DisableRaycasts()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        internal void EnableRaycasts()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        internal void Block()
        {
            _isBlock = true;
        }

        internal void Unblock()
        {
            _isBlock = false;
        }

        private void DefineBigCard()
        {
            _bigCard.Show(_cardSize, _cardRectTransform.position.x, _cardSO);
            Hide();
        }

        private void DefineSmallCard()
        {
            _bigCard.Hide();
            Show();
        }

        private void DefineSmallSize()
        {
            _cardRectTransform.sizeDelta = new Vector2(_cardSize.Width, _cardSize.Height);
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        private void Show()
        {
            _canvasGroup.alpha = 1;
        }

        private void DefineViewCharacters()
        {
            _icon.sprite = _cardSO.Icon;
            _number.text = _cardSO.Number.ToString();
            _name.text = _cardSO.Name;
            _feature.text = _cardSO.Feature;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
    }
}