using UnityEngine;

public class CardSeat : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    public CardCharacter CardCharacter { get; private set; }

    public void Init()
    {
        CardCharacter = null;
    }

    public void SetCardCharacter(CardCharacter cardCharacter)
    {
        //Vector2 cardCharacterPosition = new Vector2(_rectTransform.sizeDelta.x / 2, _rectTransform.sizeDelta.y /2);
        Vector2 cardCharacterPosition = new Vector2(0, 0);
        CardCharacter = Instantiate(cardCharacter, _rectTransform);
        CardCharacter.transform.localPosition = cardCharacterPosition;
    }

    public bool IsEmpty()
    {
        return CardCharacter == null;
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineRectTransform();
    }

    [ContextMenu(nameof(DefineRectTransform))]
    private void DefineRectTransform()
    {
        AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
    }
}
