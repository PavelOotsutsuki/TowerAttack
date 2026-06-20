using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TableActivator_OLD : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        //[SerializeField] private CanvasGroup _canvasGroup;
        //[SerializeField] private float _activateDelay = 0.5f;

        //private Coroutine _inWork;

        //public bool? IsActive { get; private set; } = null;

        //public void Activate()
        //{
        //    if (IsActive == true)
        //        return;

        //    IsActive = true;

        //    _inWork = StartCoroutine(Activating());
        //}

        //public void Deactivate()
        //{
        //    if (IsActive == false)
        //        return;

        //    IsActive = false;

        //    StartCoroutine(Deactivating());
        //}

        //private IEnumerator Activating()
        //{
        //    yield return new WaitForSeconds(_activateDelay);

        //    if (gameObject.activeSelf == false)
        //    {
        //        gameObject.SetActive(true);
        //    }

        //    _canvasGroup.blocksRaycasts = true;
        //}

        //private IEnumerator Deactivating()
        //{
        //    yield return new WaitUntil(()=> _inWork is not null);

        //    _canvasGroup.blocksRaycasts = false;
        //}

        [SerializeField] private CanvasGroup _canvasGroup;

        public bool? IsActive { get; private set; } = null;

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

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TableActivator_OLD))]
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