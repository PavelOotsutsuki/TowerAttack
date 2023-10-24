using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardSO _cardSO;
    [SerializeField] private CardDragAndDrop _cardDragAndDrop;
    [SerializeField] private CardView _cardView;
    [SerializeField] private RectTransform _rectTransform;

    public void Init(CardDescription cardDescription, BigCard bigCard)
    {
        _cardView.Init(_cardSO, _rectTransform, cardDescription, bigCard);
        _cardDragAndDrop.Init(_rectTransform, _cardView);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCardDragAndDrop();
        DefineCardView();
        DefineRectTransform();
    }

    [ContextMenu(nameof(DefineCardDragAndDrop))]
    private void DefineCardDragAndDrop()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InThis);
    }

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
