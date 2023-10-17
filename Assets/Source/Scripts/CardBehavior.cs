using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class CardBehavior : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private CardReview _cardReview;
    private string _description;
    private CardDescription _cardDescription;
    private Transform _cardTransform;
    private Table _myTable;
    private Card _card;
    private CardCharacter _cardCharacter;
    private Vector2 _dragPosition;
    private Quaternion _dragRotation;
    private bool _isDrag;

    public void Init(string description, CardDescription cardDescription, Table table, Card card, CardCharacter cardCharacter, BigCard bigCardView, CardView cardView)
    {
        _isDrag = false;
        _myTable = table;
        _card = card;
        
        _cardTransform = _card.transform;
        _description = description;
        _cardDescription = cardDescription;
        _cardCharacter = cardCharacter;
        _cardReview = new CardReview(bigCardView, cardView); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDrag == false)
        {
            _cardDescription.Show(_description);
            _cardReview.DefineBigCard(_cardTransform.position.x);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardDescription.Hide();
        _cardReview.DefineSmallCard();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDrag = true;
        _dragPosition = _cardTransform.position;
        _dragRotation = _cardTransform.rotation;
        //_cardTransfrom.rotation = Quaternion.identity;
        OnPointerExit(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        bool isSuccessSetCard = false;

        if (TryGetComponentInRaycasts(out Table table, raycastResults))
        {
            if (_myTable == table)
            {
                if (_myTable.TrySetCardCharacter(_cardCharacter))
                {
                    isSuccessSetCard = true;
                }
            }
        }
        else if (TryGetComponentInRaycasts(out CardBehavior cardBehavior, raycastResults))
        {
            if (cardBehavior == this)
            {
                OnPointerEnter(eventData);
            }
        }

        if (isSuccessSetCard)
        {
            Destroy(_card.gameObject);
        }
        else
        {
            _cardTransform.DOMove(_dragPosition,0.5f);
            //_cardTransfrom.DO(_dragRotation, 0.5f);
        }
    }

    private bool TryGetComponentInRaycasts<T>(out T findedComponent, List<RaycastResult> raycastResults) where T: class
    {
        findedComponent = null;

        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.TryGetComponent(out T component))
            {
                findedComponent = component;
                return true;
            }
        }

        return false;
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
