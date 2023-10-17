using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class CardView: MonoBehaviour
{
    [SerializeField] private float _width = 150f;
    [SerializeField] private float _height = 210f;

    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _feature;
    [SerializeField] private CanvasGroup _canvasGroup;

    private RectTransform _cardRectTransform;

    public CardSize CardSize { get; private set; }
    public CardSO CardSO { get; private set; }

    public void Init(CardSO cardSO, RectTransform cartRectTransform)
    {
        CardSize = new CardSize(_width, _height);
        _cardRectTransform = cartRectTransform;
        CardSO = cardSO;

        _icon.sprite = cardSO.Icon;
        _number.text = cardSO.Number.ToString();
        _name.text = cardSO.Name;
        _feature.text = cardSO.Feature;

        DefineSmallSize();
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
    }

    private void DefineSmallSize()
    {
        _cardRectTransform.sizeDelta = new Vector2(CardSize.Width, CardSize.Height);
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineCanvasGroup();
    }

    [ContextMenu(nameof(DefineCanvasGroup))]
    private void DefineCanvasGroup()
    {
        AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
    }
}
