using Tools.UI;

namespace GameFields.InformationLabels
{
    //[RequireComponent(typeof(FadableLabel))]
    internal class InformationLabelLabel : FadableLabel//MonoBehaviour, ICompletable, IViewable<LabelActivateData>, IAutomaticFillComponents//FadableLabel
    {
        //[SerializeField] private RectTransform _rectTransform;
        //[SerializeField] private FadableLabel _fadableLabel;

        //private Vector2 _defaultRect;


        //public bool IsComplete => _fadableLabel.IsComplete;
        //public bool? IsShown => _fadableLabel.IsShown;

        //private int TextLength => _fadableLabel.TextLength;

        //public void Init()
        //{
        //    _fadableLabel.Init();

        //    _defaultRect = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
        //    //_rectTransform.ser
        //}

        //public void Show(LabelActivateData data)
        //{
        //    _fadableLabel.Show(data);

        //    //SetRect();
        //}

        //public void Hide()
        //{
        //    _fadableLabel.Hide();
        //}

        //private void SetRect()
        //{
        //    Debug.Log(TextLength);
        //    float defaultTextLength = 78;
        //    Vector2 defaultRect = new Vector2(640f, 215f);
        //    Vector2 defaultSizeDelta = new Vector2(1280f, 865f);
        //    float rectFactor = 0.893f;

        //    float currentTextLength = TextLength;
        //    float currentFactor = defaultTextLength / currentTextLength / rectFactor;

        //    Vector2 newRect = defaultRect / currentFactor;

        //    Vector2 resultRect = new Vector2(newRect.x > _defaultRect.x ? newRect.x : _defaultRect.x, newRect.y > _defaultRect.y ? newRect.y : _defaultRect.y);

        //    float rightDiff = resultRect.x - _defaultRect.x;
        //    float bottomDiff = resultRect.y - _defaultRect.y;

        //    _rectTransform.offsetMax = new Vector2(-1f * (defaultSizeDelta.x - rightDiff), 0f); 
        //    _rectTransform.offsetMin = new Vector2(0f, defaultSizeDelta.y - bottomDiff);

        //    //_rectTransform.sizeDelta = new Vector2(defaultSizeDelta.x - rightDiff, defaultSizeDelta.y - bottomDiff);
        //    //_rectTransform.rect.Set(_rectTransform.rect.x, _rectTransform.rect.y, resultRect.x, resultRect.y);
        //    Debug.Log(newRect + " and " + resultRect + " and " + _rectTransform.sizeDelta);
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(InformationLabelLabel))]
        //public List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineRectTransform(),
        //        DefineFadableLabel()
        //    };

        //    return list;
        //}

        //[ContextMenu(nameof(DefineRectTransform))]
        //private ComponentAttachInfo DefineRectTransform()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        //}

        //[ContextMenu(nameof(DefineFadableLabel))]
        //private ComponentAttachInfo DefineFadableLabel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        //}
        //#endregion
    }
}