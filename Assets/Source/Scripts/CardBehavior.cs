using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehavior : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] private CardReview _cardReview;

    private string _description;
    private CardDescription _cardDescription;
    private RectTransform _cardSourceTransform;

    public void Init(string description, CardDescription cardDescription, RectTransform siblingTransform, RectTransform cardSourceTransform)
    {
        _cardSourceTransform = cardSourceTransform;
        _description = description;
        _cardDescription = cardDescription;
        _cardReview.Init(_cardSourceTransform, siblingTransform); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardDescription.Show(_description);
        _cardReview.DefineBigCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardDescription.Hide();
        _cardReview.DefineSmallCard();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardSourceTransform.position = eventData.position;
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCardReview();
    }

    [ContextMenu(nameof(DefineCardReview))]
    private void DefineCardReview()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardReview, ComponentLocationTypes.InThis);
    }
}
