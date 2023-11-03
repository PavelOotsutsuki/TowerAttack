using UnityEngine;
using Tools;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private CardSO _cardSO;
        //[SerializeField] private CardDragAndDrop _cardDragAndDrop;
        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _translateSpeed;

        internal void Init(CardDescription cardDescription, BigCard bigCard, bool isBackView = true)
        {
            _cardSO.CardCharacter.Init(_cardSO.AwakeSound);
            _cardView.Init(_cardSO, _rectTransform, cardDescription, bigCard, isBackView);
            //_cardDragAndDrop.Init(_rectTransform, _cardView.CardFront);
        }

        public void AddToTable(out CardCharacter cardCharacter)
        {
            cardCharacter = _cardSO.CardCharacter;
            Destroy();
        }

        public void TranslateLocalInto(Vector2 positon, Vector3 rotation, float duration)
        {
            _cardView.TranslateLocalInto(positon, rotation, duration);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            //DefineCardDragAndDrop();
            DefineCardView();
            DefineRectTransform();
        }

        //[ContextMenu(nameof(DefineCardDragAndDrop))]
        //private void DefineCardDragAndDrop()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InThis);
        //}

        [ContextMenu(nameof(DefineCardView))]
        private void DefineCardView()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardView, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
    }
}
