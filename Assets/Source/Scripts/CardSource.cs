using UnityEngine;

public class CardSource : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CardView _cardView;

    public RectTransform RectTransform => _rectTransform;

    public void Init(CardSO cardSO)
    {
        _cardView.Init(cardSO);
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCardView();
        DefineRectTransform();
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
