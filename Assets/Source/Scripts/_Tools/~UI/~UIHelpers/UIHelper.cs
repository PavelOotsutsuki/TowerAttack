using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Tools.UI.UIHelpers
{
    public class UIHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;

        private UIHelperDescription _UIHelperDescription;
        private Func<string> _textDescriptionGetter;

        private Coroutine _activatingCoroutine;
        private Coroutine _deactivatingCoroutine;

        //[Inject]
        //private void Construct(UIHelperDescription UIHelperDescription)
        //{
        //    _UIHelperDescription = UIHelperDescription;
        //}

        //public void OnEnable()
        //{
        //    Object[] targets = Object.FindObjectsOfType(typeof(UIHelperDescription));
        //    _UIHelperDescription = (UIHelperDescription)targets[0];
        //}

        public void Init(UIHelperDescription UIHelperDescription, Func<string> textDescriptionGetter)
        {
            _UIHelperDescription = UIHelperDescription;
            _textDescriptionGetter = textDescriptionGetter;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_activatingCoroutine != null)
            {
                StopCoroutine(_activatingCoroutine);
                _activatingCoroutine = null;
            }

            _activatingCoroutine = StartCoroutine(Activating());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_activatingCoroutine != null)
            {
                StopCoroutine(_activatingCoroutine);
                _activatingCoroutine = null;
            }

            _deactivatingCoroutine = StartCoroutine(Deactivating());
        }

        private IEnumerator Activating()
        {
            if (_activatingCoroutine != null)
            {
                StopCoroutine(_activatingCoroutine);
                _activatingCoroutine = null;
                Debug.Log("Никогда не произойдет!");
            }
            //else
            //{
            //    yield return new WaitForSeconds(0.8f);
            //}

            if (_deactivatingCoroutine != null)
            {
                StopCoroutine(_deactivatingCoroutine);
                _deactivatingCoroutine = null;
            }
            else
            {
                yield return new WaitForSeconds(0.8f);
            }

            UIHelperDescriptionActivateData activateData = new UIHelperDescriptionActivateData(_textDescriptionGetter.Invoke(), new ReadOnlyRectTransform(_rectTransform));
            _UIHelperDescription.Activate(activateData);

            yield return new WaitUntil(() => _UIHelperDescription.IsComplete);

            _activatingCoroutine = null;
        }

        private IEnumerator Deactivating()
        {
            _UIHelperDescription.Deactivate();

            yield return new WaitUntil(() => _UIHelperDescription.IsComplete);

            _deactivatingCoroutine = null;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(UIHelper))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}