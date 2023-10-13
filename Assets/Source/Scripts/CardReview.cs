using UnityEngine;

public class CardReview : MonoBehaviour
{
    [SerializeField, Min(1f)] private float _scaleFactor = 2f;
    [SerializeField] private float _reviewOffsetFactor = 2f;
    [SerializeField] private float _smallWidth = 250f;
    [SerializeField] private float _smallHeight = 350f;

    private RectTransform _siblingTransform;
    private int _siblingIndex;
    private Quaternion _rotation;
    private RectTransform _cardSourceTransform;
    private Vector2 _startPosition;

    public void Init(RectTransform cardSourceTransform, RectTransform siblingTransform)
    {
        _cardSourceTransform = cardSourceTransform;
        _siblingTransform = siblingTransform;
        _siblingIndex = _siblingTransform.GetSiblingIndex();
        _rotation = _cardSourceTransform.rotation;
        _startPosition = _cardSourceTransform.localPosition;

        DefineSmallSize();
    }

    public void DefineSmallCard()
    {
        _siblingTransform.SetSiblingIndex(_siblingIndex);
        DefineSmallSize();
        _cardSourceTransform.localPosition = _startPosition;
    }

    public void DefineBigCard()
    {
        _siblingIndex = _siblingTransform.GetSiblingIndex();
        _siblingTransform.SetAsLastSibling();
        _startPosition = _cardSourceTransform.localPosition;
        _cardSourceTransform.sizeDelta = new Vector2(_smallWidth * _scaleFactor, _smallHeight * _scaleFactor);
        _cardSourceTransform.localPosition = new Vector2(_cardSourceTransform.localPosition.x, _smallHeight * _scaleFactor / _reviewOffsetFactor);
        _cardSourceTransform.rotation = Quaternion.identity;
    }

    private void DefineSmallSize()
    {
        _cardSourceTransform.sizeDelta = new Vector2(_smallWidth, _smallHeight);
        _cardSourceTransform.rotation = _rotation;
    }
}
