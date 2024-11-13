using System.Collections;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TableActivator : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _activateDelay = 0.5f;

        private Coroutine _inWork;

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _inWork = StartCoroutine(Activating());
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            StartCoroutine(Deactivating());
        }

        private IEnumerator Activating()
        {
            yield return new WaitForSeconds(_activateDelay);

            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            _canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(()=> _inWork is not null);

            _canvasGroup.blocksRaycasts = false;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TableActivator))]
        public void DefineAllComponents()
        {
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}