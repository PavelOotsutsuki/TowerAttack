using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.Settings;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class BigCardDescription : FadableLabel//, IShowable<BigCardDescriptionActivateData>
    {
        //[SerializeField] private RectTransform _rectTransform;

        //private ReadOnlyRectTransform _readOnlyRectTransform;
        //private float _width;

        //private Coroutine _currentCoroutine;

        //public override void Init()
        //{
        //    base.Init();

            //_readOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
            //_width = _readOnlyRectTransform.GetWidth();
        //}

        //public void Show(BigCardDescriptionActivateData data)
        //{
            //float xPosition;
            //_readOnlyRectTransform.SetSize(new Vector2(data.BigCardSize.x * 2f, data.BigCardSize.y * 2f));

            //if (data.BigCardPosition.x + (data.BigCardSize.x / 2f + data.BigCardSize.x) > GameSettings.CanvasReferenceResolution.x - 10)
            //{
            //    xPosition = data.BigCardPosition.x - (data.BigCardSize.x / 2f + data.BigCardSize.x);
            //}
            //else
            //{
            //    xPosition = data.BigCardPosition.x + (data.BigCardSize.x / 2f + data.BigCardSize.x);
            //}

            //_rectTransform.position = new Vector2(xPosition, data.BigCardPosition.y);

            //if (_currentCoroutine != null)
            //{
            //    StopCoroutine(_currentCoroutine);
            //    //_currentCoroutine = StartCoroutine(WaitingUntilShow(data.LabelActivateData, false));
            //}

            //_currentCoroutine = StartCoroutine(WaitingUntilShow(data.LabelActivateData, IsComplete));

        //    base.Show(data.LabelActivateData);
        //}

        //public override void Hide()
        //{
            //if (_currentCoroutine != null)
                //StopCoroutine(_currentCoroutine);

        //    base.Hide();
        //}

        //private IEnumerator WaitingUntilShow(LabelActivateData activateData, bool isWait)
        //{
        //    if (isWait)
        //        yield return new WaitForSeconds(1f);

        //    base.Show(activateData);

        //    _currentCoroutine = null;
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(BigCardDescription))]
        //public override List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineRectTransform()
        //    };

        //    list.AddRange(base.DefineAllComponents());

        //    return list;
        //}

        //[ContextMenu(nameof(DefineRectTransform))]
        //private ComponentAttachInfo DefineRectTransform()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        //}
        //#endregion 
    }
}
