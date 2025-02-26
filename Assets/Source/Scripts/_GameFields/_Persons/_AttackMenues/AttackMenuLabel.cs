using Tools.UI;

namespace GameFields.Persons.AttackMenues
{
    //[RequireComponent(typeof(FadableLabel))]
    public class AttackMenuLabel : FadableLabel //FadableNascentLabel
    {
        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenuLabel))]
        //protected override void DefineAllComponents()
        //{
        //    base.DefineAllComponents();
        //}
        //#endregion 
        //[SerializeField] private FadableLabel _fadableLabel;

        //public bool IsComplete => _fadableLabel.IsComplete;

        //public void Init()
        //{
        //    _fadableLabel.Init();
        //}

        //public void Activate(FadableLabelActivateData data)
        //{
        //    _fadableLabel.Show(data);
        //}

        //public void Deactivate()
        //{
        //    _fadableLabel.Hide();
        //}

        //#region AutomaticFillComponents

        //[ContextMenu(nameof(DefineAllComponents))]
        //private void DefineAllComponents()
        //{
        //    DefineFadableLabel();
        //}

        //[ContextMenu(nameof(DefineFadableLabel))]
        //private void DefineFadableLabel()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        //}

        //#endregion
        //private const float LifeAlpha = 1f;
        //private const float EndAlpha = 0f;

        //[SerializeField] private TMP_Text _label;
        //[SerializeField] private float _fontSize = 60f;
        //[SerializeField] private Vector2 _startScale = Vector2.zero;
        //[SerializeField] private Vector2 _endScale = new Vector2(1f, 1f);
        //[SerializeField] private float _duration = 0.3f;

        //public void Init()
        //{
        //    _label.fontSize = _fontSize;

        //    Deactivate();
        //}

        //public void Activate(AttackMenuLabelActivateData data)
        //{
        //    gameObject.SetActive(true);
        //    _label.text = data.Message;

        //    Color endColor = new Color(_label.color.r, _label.color.g, _label.color.b, LifeAlpha);

        //    _label.DOColor(endColor, _duration);
        //    _label.transform.DOScale(_endScale, _duration);
        //}

        //public void Deactivate()
        //{
        //    gameObject.SetActive(false);

        //    Color startColor = new Color(_label.color.r, _label.color.g, _label.color.b, EndAlpha);

        //    _label.color = startColor;
        //    _label.transform.localScale = _startScale;
        //}

        //private IEnumerator Activating()
        //{
        //    //gameObject.SetActive(true);
        //    ////_label.text = message;

        //    //float startFontSize = _label.fontSize;
        //    //float fontSizeWay = (_endFontSize - startFontSize) / _duration;

        //    //for (float time = 0f; time < _duration; time += Time.deltaTime)
        //    //{
        //    //    _label.fontSize = startFontSize + fontSizeWay * time;
        //        yield return null;
        //    //}

        //    //startFontSize = _label.fontSize;
        //    //fontSizeWay = (_endFontSize - startFontSize) / _endDuration;

        //    //float startAlpha = _label.color.a;
        //    //float alphaWay = (EndAlpha - startAlpha) / _endDuration;

        //    //Color color = new(_label.color.r, _label.color.g, _label.color.b, startAlpha);

        //    //for (float time = 0f; time < _endDuration; time += Time.deltaTime)
        //    //{
        //    //    color.a = startAlpha + alphaWay * time;
        //    //    _label.color = color;
        //    //    _label.fontSize = startFontSize + fontSizeWay * time;
        //    //    yield return null;
        //    //}

        //    //gameObject.SetActive(false);
        //}

        //#region AutomaticFillComponents

        //[ContextMenu(nameof(DefineAllComponents))]
        //private void DefineAllComponents()
        //{
        //    DefineLabel();
        //}

        //[ContextMenu(nameof(DefineLabel))]
        //private void DefineLabel()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        //}

        //#endregion 
    }
}