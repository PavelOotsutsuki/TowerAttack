using System.Collections.Generic;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadableLabel))]
    public class UIHelperDescription : MonoBehaviour, ICompletable, IWorkable<UIHelperDescriptionActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private FadableLabel _fadableLabel;
        [SerializeField] private RectTransform _rectTransform;

        private float _thisWidth;
        private float _thisHeight;

        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _fadableLabel.IsComplete;

        public void Init()
        {
            gameObject.SetActive(true);

            _fadableLabel.Init();

            _thisWidth = _rectTransform.rect.width * _rectTransform.localScale.x;
            _thisHeight = _rectTransform.rect.height * _rectTransform.localScale.y;
        }

        public void Activate(UIHelperDescriptionActivateData data)
        {
            IsActive = true;

            //_rectTransform.position = data.LogicChildTransform;
            SetPosition(data.LogicChildTransform);

            LabelActivateData labelActivateData = data.LabelActivateData;
            _fadableLabel.Show(labelActivateData);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _fadableLabel.Hide();
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
                DefineFadableLabel(),
                DefineRectTransform()
            };

            return list;
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