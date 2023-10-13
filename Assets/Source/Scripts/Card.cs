using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardSO _cardSO;
    [SerializeField] private CardTrigger _cardTrigger;
    [SerializeField] private CardSource _cardSource;
    [SerializeField] private RectTransform _rectTransform;

    public void Init(CardDescription cardDescription)
    {
        _cardSource.Init(_cardSO);
        _cardTrigger.Init(_cardSO.Description, cardDescription, _rectTransform, _cardSource.RectTransform);
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCardTrigger();
        DefineCardSource();
        DefineRectTransform();
    }

    [ContextMenu(nameof(DefineCardTrigger))]
    private void DefineCardTrigger()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardTrigger, ComponentLocationTypes.InChildren);
    }

    [ContextMenu(nameof(DefineCardSource))]
    private void DefineCardSource()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardSource, ComponentLocationTypes.InChildren);
    }

    [ContextMenu(nameof(DefineRectTransform))]
    private void DefineRectTransform()
    {
        AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
    }
}
