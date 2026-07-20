using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI.UIHelpers
{
    public class UIHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;

        private UIHelperDescription _UIHelperDescription;
        private Func<string> _textDescriptionGetter;

        private CancellationToken _gameFieldToken;
        private CancellationTokenSource _hiddingCTS;
        private CancellationTokenSource _showingCTS;
        //private Coroutine _activatingCoroutine;
        //private Coroutine _deactivatingCoroutine;

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

        public void Init(UIHelperDescription UIHelperDescription, Func<string> textDescriptionGetter, CancellationToken gameFieldToken)
        {
            _UIHelperDescription = UIHelperDescription;
            _textDescriptionGetter = textDescriptionGetter;
            _gameFieldToken = gameFieldToken;

            //gameObject.SetActive(false); // не надо, иначе скрывает сами иконки
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_gameFieldToken.IsCancellationRequested)
                return;

            Utils.Utils.DestroyCTS(ref _showingCTS);
            _showingCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameFieldToken);

            Activating(_showingCTS.Token).Forget();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_gameFieldToken.IsCancellationRequested)
                return;

            Utils.Utils.DestroyCTS(ref _showingCTS);
            Utils.Utils.DestroyCTS(ref _hiddingCTS);
            _hiddingCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameFieldToken);

            Deactivating(_hiddingCTS.Token).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            if (_gameFieldToken.IsCancellationRequested)
                return;

            try
            {
                //gameObject.SetActive(true);

                if (_hiddingCTS != null)
                {
                    Utils.Utils.DestroyCTS(ref _hiddingCTS);
                }
                else
                {
                    await UniTask.WaitForSeconds(0.8f, cancellationToken: token);
                }

                UIHelperDescriptionActivateData activateData = new UIHelperDescriptionActivateData(_textDescriptionGetter.Invoke(), new ReadOnlyRectTransform(_rectTransform));
                _UIHelperDescription.Activate(activateData);

                await UniTask.WaitUntil(() => _UIHelperDescription.IsComplete, cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }

            //if (_deactivatingCoroutine != null)
            //{
            //    StopCoroutine(_deactivatingCoroutine);
            //    _deactivatingCoroutine = null;
            //}
            //else
            //{
            //    yield return new WaitForSeconds(0.8f);
            //}

            //UIHelperDescriptionActivateData activateData = new UIHelperDescriptionActivateData(_textDescriptionGetter.Invoke(), new ReadOnlyRectTransform(_rectTransform));
            //_UIHelperDescription.Activate(activateData);

            //yield return new WaitUntil(() => _UIHelperDescription.IsComplete);

            //_activatingCoroutine = null;
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            //_UIHelperDescription.Deactivate();

            //await UniTask.WaitUntil(() => _UIHelperDescription.IsComplete, cancellationToken: token);

            //_deactivatingCoroutine = null;

            if (_gameFieldToken.IsCancellationRequested)
                return;

            try
            {
                _UIHelperDescription.Deactivate();

                await UniTask.WaitUntil(() => _UIHelperDescription.IsComplete, cancellationToken: token);

                //gameObject.SetActive(false);
                Utils.Utils.DestroyCTS(ref _hiddingCTS);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
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