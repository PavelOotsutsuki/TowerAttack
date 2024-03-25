using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        private CardSO _cardSo;

        internal void Init(CardSO cardSo, RectTransform cartRectTransform, CardDescription cardDescription, BigCard bigCard)
        {
            enabled = false;
            _cardSize = new CardSize(_width, _height);
            _cardRectTransform = cartRectTransform;
            _cardDescription = cardDescription;
            _bigCard = bigCard;
            _cardSo = cardSo;

            DefineViewCharacters();
            DefineSmallSize();
        }

        internal void StartReview()
        {
            _cardDescription.Show(_cardSo.Description);
            DefineBigCard();
        }

        internal void EndReview()
        {
            _cardDescription.Hide();
            DefineSmallCard();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartReview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EndReview();
        }

        internal void Disable()
        {
            enabled = false;
        }

        internal void Enable()
        {
            enabled = true;

            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] components = Physics2D.RaycastAll(ray, Vector3.forward);

            foreach (RaycastHit2D raycastHit in components)
            {
                if (raycastHit.collider.gameObject.TryGetComponent(out CardFront cardFront))
                {
                    if (cardFront == this)
                    {
                        Debug.Log("УРа");
                        StartReview();
                    }
                }
            }
        }

        private void DefineBigCard()
        {
            _bigCard.Show(_cardSize, _cardRectTransform.position.x, _cardSo);
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
            _icon.sprite = _cardSo.Icon;
            _number.text = _cardSo.Number.ToString();
            _name.text = _cardSo.Name;
            _feature.text = _cardSo.Feature;
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