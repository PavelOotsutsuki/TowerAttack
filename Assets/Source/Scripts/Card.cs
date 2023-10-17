using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardSO _cardSO;
    [SerializeField] private CardBehavior _cardBehavior;
    [SerializeField] private CardView _cardView;
    [SerializeField] private RectTransform _rectTransform;

    public void Init(CardDescription cardDescription, BigCard bigCard, Table table)
    {
        _cardView.Init(_cardSO, _rectTransform);
        _cardBehavior.Init(_cardSO.Description, cardDescription, table, this, _cardSO.CardCharacter, bigCard, _cardView);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCardBehavior();
        DefineCardView();
        DefineRectTransform();
    }

    [ContextMenu(nameof(DefineCardBehavior))]
    private void DefineCardBehavior()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardBehavior, ComponentLocationTypes.InThis);
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
