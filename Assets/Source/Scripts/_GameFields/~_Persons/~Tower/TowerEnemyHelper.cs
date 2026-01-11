using System;
using System.Collections.Generic;
using Cards;
using Cards.Views;
using GameFields.Persons.ConfirmableNumbersView;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.Towers
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TowerEnemyHelper : MonoBehaviour, IWorkable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private ConfirmableNumbersViewRoot _confirmableNumbersViewRoot;

        public bool? IsActive { get; private set; } = null;

        public void Init(ConfirmableNumbersViewRoot confirmableNumbersViewRoot)
        {
            _confirmableNumbersViewRoot = confirmableNumbersViewRoot;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;
            //Debug.Log("TowerEnemyHelper.Activate");

            _canvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            //Debug.Log("TowerEnemyHelper.Deactivate");

            _canvasGroup.blocksRaycasts = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("TowerEnemyHelper.OnPointerEnter");
            _confirmableNumbersViewRoot.Activate();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("TowerEnemyHelper.OnPointerExit");
            _confirmableNumbersViewRoot.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerPlayerHelper))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}