using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    public class DiscoverCardFront : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;
        [SerializeField] private CanvasGroup _canvasGroup;

        private CardDescription _cardDescription;
        private CardSize _cardSize;
        private CardSO _cardSO;
        private bool _isBlock;

        public bool IsBlock => _isBlock;

        internal void Init(CardSO cardSO, CardDescription cardDescription)
        {
            _isBlock = false;
            _cardDescription = cardDescription;
            _cardSO = cardSO;

            DefineViewCharacters();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isBlock)
            {
                return;
            }

            StartReview();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isBlock)
            {
                return;
            }

            EndReview();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //
        }

        //internal void TranslateInto(Vector2 positon, Vector3 rotation, float duration)
        //{
        //    _cardRectTransform.DOMove(positon, duration);
        //    _cardRectTransform.DORotate(rotation, duration);
        //}

        internal void Block()
        {
            _isBlock = true;
        }

        internal void Unblock()
        {
            _isBlock = false;

            //PointerEventData eventData = new PointerEventData(EventSystem.current);
            //if (EventSystem.current.TryGetComponentInRaycasts(eventData, out CardFront cardFront))
            //{
            //    if (cardFront == this)
            //    {
            //        StartReview();
            //    }
            //}


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

        private void StartReview()
        {
            _cardDescription.Show(_cardSO.Description);
        }

        private void EndReview()
        {
            _cardDescription.Hide();
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
