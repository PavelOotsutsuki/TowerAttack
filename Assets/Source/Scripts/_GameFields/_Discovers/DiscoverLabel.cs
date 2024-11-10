using DG.Tweening;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    //[RequireComponent(typeof(FadableLabel))]
    public class DiscoverLabel : FadableLabel//MonoBehaviour, ICompletable, IWorkable<FadableLabelActivateData>
    {
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

        //public void Activate(DiscoverLabelActivateData data)
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