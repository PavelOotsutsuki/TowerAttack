using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private float _returnInHandSpeed;

    private Transform _cardTransform;
    //private Table _myTable;
    private CardView _cardView;
    //private CardCharacter _cardCharacter;
    private Vector2 _dragPosition;
    private Vector3 _dragRotation;
    private int _siblingIndex;
    private Coroutine _viewCardAfterDropInWork;
    private bool _isDrag;
    private bool _isAlreadyDrag;



    public void Init(RectTransform cardTransform, CardView cardView)
    {
        //_myTable = table;
        _isDrag = false;
        _isAlreadyDrag = false;
        _cardView = cardView;

        _cardTransform = cardTransform;
        //_cardCharacter = cardCharacter;
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (_isDrag == false)
    //    {
    //        _cardDescription.Show(_description);
    //        _cardReview.DefineBigCard(_cardTransform.position.x);
    //    }
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    _cardDescription.Hide();
    //    _cardReview.DefineSmallCard();
    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isDrag)
        {
            _isAlreadyDrag = true;
            return;
        }


        ActivateStartDragOptions();

        _cardView.OnPointerExit(eventData);
        DefineDragTransformValues();
        DefineDragSiblingIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //List<RaycastResult> raycastResults = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventData, raycastResults);

        //bool isSuccessSetCard = false;
        if (_isAlreadyDrag)
        {
            return;
        }

        _cardTransform.SetSiblingIndex(_siblingIndex);

        //_cardTransform.SetSiblingIndex(_siblingIndex);
        //IsDrag = false;

        //if (TryGetComponentInRaycasts(out Table table, raycastResults))
        //{
        //    if (_myTable == table)
        //    {
        //        if (_myTable.TrySetCardCharacter(_cardCharacter))
        //        {
        //            isSuccessSetCard = true;
        //        }
        //    }
        //}
        //else if (TryGetComponentInRaycasts(out CardDragAndDrop cardBehavior, raycastResults))
        //{
        //    if (cardBehavior == this)
        //    {
        //        _cardView.OnPointerEnter(eventData);
        //    }
        //}
        if (_viewCardAfterDropInWork != null)
        {
            StopCoroutine(_viewCardAfterDropInWork);
        }

        _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_returnInHandSpeed, eventData));

        //if (isSuccessSetCard == false)
        //{
            _cardTransform.DOMove(_dragPosition, _returnInHandSpeed);
            _cardTransform.DORotate(_dragRotation, _returnInHandSpeed);
        //}
    }

    private void ActivateStartDragOptions()
    {
        _isDrag = true;
        _cardView.Block();
    }

    private void ActivateEndDragOptions()
    {
        _isDrag = false;
        _cardView.Unblock();
    }

    private IEnumerator ViewCardAfterDrop(float endDuration, PointerEventData eventData)
    {
        for (float duration = 0; duration < endDuration; duration += Time.deltaTime)
        {
            yield return true;
        }

        Debug.Log("corutine");
        //_isAlreadyDrag = false;
        ActivateEndDragOptions();

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        if (TryGetComponentInRaycasts(out CardDragAndDrop cardDragAndDrop, raycastResults))
        {
            if (cardDragAndDrop == this)
            {
                _cardView.OnPointerEnter(eventData);
            }
        }
    }

    private void DefineDragSiblingIndex()
    {
        _siblingIndex = _cardTransform.GetSiblingIndex();
        _cardTransform.SetAsLastSibling();
    }

    private void DefineDragTransformValues()
    {
        _dragPosition = _cardTransform.position;
        _dragRotation = _cardTransform.rotation.eulerAngles;
        _cardTransform.rotation = Quaternion.identity;
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

    public void OnDrop(PointerEventData eventData)
    {
    }
}
