using System.Collections.Generic;
using System.Threading;
using TMPro;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;

namespace Tools.UI.UIHelpers
{
    [RequireComponent(typeof(FadableLabel))]
    public class UIHelperDescription : MonoBehaviour, ICompletable, IWorkable<UIHelperDescriptionActivateData>, IAutomaticFillComponents
    {
        //private const float ExtraWidth = 205.49f;
        //private const float ExtraHeight = 156.54f;
        //private const float ExtraWidth = 0f;
        //private const float ExtraHeight = 0f;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private FadableLabel _fadableLabel;
        [SerializeField] private RectTransform _rectTransform;

        private float _thisWidth;
        private float _thisHeight;
        private Vector3 _startScale;
        private float _rectProportion;
        private CancellationToken _fightToken;

        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _fadableLabel.IsComplete;

        public void Init(CancellationToken fightToken)
        {
            _fightToken = fightToken;

            gameObject.SetActive(true);

            _fadableLabel.Init();

            // С изначальным Scale-ом отличным от 1 не работает. впадлу придумывать логику для него, если итак сойдет
            _rectTransform.localScale = new Vector3(1f, 1f, 1f);

            _startScale = _rectTransform.localScale;
            _rectProportion = _rectTransform.rect.width / _rectTransform.rect.height;

            _thisWidth = _rectTransform.rect.width * _startScale.x;
            _thisHeight = _rectTransform.rect.height * _startScale.y;

            //Debug.Log($"_startScale={_startScale}\n_rectProportion={_rectProportion}\n_thisWidth={_thisWidth}\n_thisHeight={_thisHeight}\n_rectTransform.rect.width={_rectTransform.rect.width}\n_rectTransform.rect.height={_rectTransform.rect.height}");
        }

        public void SetText(string text)
        {
            _fadableLabel.SetText(text);
        }

        // TODO: Нужно ли по итогу _rectTransform.SetSizeWithCurrentAnchors хз, с ним работает, без него лень проверять
        public void Activate(UIHelperDescriptionActivateData data)
        {
            IsActive = true;

            //_rectTransform.position = data.LogicChildTransform;

            LabelActivateDataAsync labelActivateDataAsync = new LabelActivateDataAsync(data.LabelActivateData, _fightToken);
            _fadableLabel.Show(labelActivateDataAsync);

            // После смены text-a надо поменять width, иначе preferredHeight нормально не расчитывается
            float startWidth = _rectTransform.rect.width;
            //Debug.Log($"BEFORE: _rectTransform.rect.width={_rectTransform.rect.width}\n_rectTransform.rect.height={_rectTransform.rect.height}\n_text.preferredWidth={_text.preferredWidth}");
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _text.preferredWidth);

            //Debug.Log($"AFTER: _rectTransform.rect.width={_rectTransform.rect.width}\n_rectTransform.rect.height={_rectTransform.rect.height}\n_text.preferredWidth={_text.preferredWidth}");

            float preferredWidth = _text.preferredWidth;

            float square = preferredWidth * _rectTransform.rect.height;
            float newHeight = Mathf.Sqrt(square / _rectProportion);
            Vector3 newScale = (newHeight / _rectTransform.rect.height) * _startScale;

            //Debug.Log($"preferredWidth={preferredWidth}\nsquare={square}\nnewHeight={newHeight}newScale={newScale}");

            _rectTransform.localScale = newScale;

            //float width = ExtraWidth;
            //float height = _text.preferredHeight + ExtraHeight;

            //_rectTransform.sizeDelta = new Vector2(width, height);

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, startWidth);


            SetPosition(data.LogicChildTransform);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _fadableLabel.Hide(new CancellationTokenData(_fightToken));
        }

        private void SetPosition(ReadOnlyRectTransform RORectTransform)
        {
            float xFactor = ScreenView.GetFactorX();
            float yFactor = ScreenView.GetFactorY();

            Vector3 childPosition = RORectTransform.GetPosition();
            float childWidth = RORectTransform.GetWidth() * xFactor;
            float childHeight = RORectTransform.GetHeight() * yFactor;

            float xFullOffset = childWidth * 0.5f + _thisWidth * xFactor * 0.5f;
            float yFullOffset = childHeight * 0.5f + _thisHeight * yFactor * 0.5f;

            float xDirection = (childPosition.x + xFullOffset + (_thisWidth * xFactor)) > ScreenView.X() * xFactor ? -1 : 1;
            float yDirection = (childPosition.y + yFullOffset + (_thisHeight * yFactor)) > ScreenView.Y() * yFactor ? -1 : 1;

            float positionX = childPosition.x + (xFullOffset * xDirection);
            float positionY = childPosition.y + (yFullOffset * yDirection);

            //Debug.Log($"xFactor = {xFactor}\nyFactor = {yFactor}\nchildPosition = {childPosition}\nchildWidth = {childWidth}\nchildHeight = {childHeight}\n" +
            //    $"positionX = {positionX}\npositionY{positionY}\n_thisWidth = {_thisWidth}\n_thisHeight = {_thisHeight}");
            //Debug.Log($"_rectTransform.position BEFORE = {_rectTransform.position}");
            _rectTransform.position = new Vector3(positionX, positionY, _rectTransform.position.z);
            //Debug.Log($"_rectTransform.position AFTER = {_rectTransform.position}");

        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(UIHelperDescription))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTMP_Text(),
                DefineFadableLabel(),
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTMP_Text))]
        private ComponentAttachInfo DefineTMP_Text()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private ComponentAttachInfo DefineFadableLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}