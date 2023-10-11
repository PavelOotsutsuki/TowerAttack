using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IDragHandler, IBeginDragHandler
{
    private const float SmallWidth = 2.5f;
    private const float SmallHeight = 3.5f;
    private const float BigWidth = 5f;
    private const float BigHeight = 7f;

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CardSO _cardSO;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _feature;
    [SerializeField] private AudioSource _audioSource;

    private Camera _mainCamera;
    private CardDescription _cardDescription;
    private CardView _cardView;
    private CardBehavior _cardBehavior;

    public void Init(CardDescription cardDescription, Camera mainCamera)
    {
        _cardDescription = cardDescription;
        _mainCamera = mainCamera;

        _cardView = new CardView(_cardSO, _icon, _number, _name, _feature, _audioSource);
        _cardBehavior = new CardBehavior(_audioSource);

        DefineSmallCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       // _audioSource.Play();
        _cardDescription.Show(_cardSO.Description);
        DefineBigCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardDescription.Hide();
        DefineSmallCard();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform.SetParent(_mainCamera.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(Input.mousePosition);
        Debug.Log(eventData.position);
        _rectTransform.position = eventData.position;
        //_rectTransform.position = _mainCamera.ViewportToWorldPoint(Input.mousePosition);
    }

    private void DefineSmallCard()
    {
        _rectTransform.sizeDelta = new Vector2(SmallWidth, SmallHeight);
    }

    private void DefineBigCard()
    {
        _rectTransform.sizeDelta = new Vector2(BigWidth, BigHeight);
    }
}
