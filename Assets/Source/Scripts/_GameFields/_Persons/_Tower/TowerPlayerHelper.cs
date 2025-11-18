using System;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.Towers
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TowerPlayerHelper : MonoBehaviour, IWorkable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TowerCardView _towerCardView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private ReadOnlyRectTransform _readOnlyRectTransform;
        private Func<CardViewData> _configGetter;

        public bool? IsActive { get; private set; } = null;

        public void Init(Func<CardViewData> configGetter)
        {
            _readOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
            _towerCardView.Init();
            _configGetter = configGetter;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _canvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _canvasGroup.blocksRaycasts = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter");
            TowerBigCardShowData data = new TowerBigCardShowData(GameSettings.CardSize, _readOnlyRectTransform, _configGetter.Invoke());
            _towerCardView.Activate(data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit");
            _towerCardView.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerPlayerHelper))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerCardView(),
                DefineRectTransform(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerCardView))]
        private ComponentAttachInfo DefineTowerCardView()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _towerCardView, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}
