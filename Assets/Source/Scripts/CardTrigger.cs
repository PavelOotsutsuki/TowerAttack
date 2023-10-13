using UnityEngine;

[RequireComponent(typeof(CardBehavior))]
public class CardTrigger : MonoBehaviour
{
    [SerializeField] private CardBehavior _cardBehavior;

    public void Init(string description, CardDescription cardDescription, RectTransform siblingTransform, RectTransform cardSourceTransform)
    {
        _cardBehavior.Init(description, cardDescription, siblingTransform, cardSourceTransform);
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCardBehavior();
    }

    [ContextMenu(nameof(DefineCardBehavior))]
    private void DefineCardBehavior()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardBehavior, ComponentLocationTypes.InThis);
    }
}
