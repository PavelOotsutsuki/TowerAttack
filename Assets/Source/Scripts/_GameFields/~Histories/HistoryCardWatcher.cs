using System;
using System.Collections.Generic;
using Cards.Views;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.BigCards;
using Cysharp.Threading.Tasks;
using TMPro;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tools.Extensions;


namespace GameFields.Histories
{
    public class HistoryCardWatcher : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IAutomaticFillComponents
    {
        [SerializeField] private TextMeshProUGUI _historyText;

        private CardViewService _cardViewService;
        private ReadOnlyRectTransform _historyMenuRORTransform;
        private IHistoryCardLinkHandler _historyCardLinkHandler;

        private bool _isActive;
        private Vector2 _lastPosition = Vector2.zero;
        //private CardViewData _currentCardViewData;

        public void Init(CardViewService cardViewService, ReadOnlyRectTransform historyMenuRORTransform, IHistoryCardLinkHandler historyCardLinkHandler)
        {
            _cardViewService = cardViewService;
            _historyMenuRORTransform = historyMenuRORTransform;
            _historyCardLinkHandler = historyCardLinkHandler;

            _isActive = false;
        }

        public void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(HistoryText_OnTextChanged);
        }

        public void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(HistoryText_OnTextChanged);
        }

        private void HistoryText_OnTextChanged(UnityEngine.Object obj)
        {
            if (obj == _historyText)
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

                //Debug.Log("HistoryText_OnTextChanged");
                CheckOverviewNextFrame(pointerEventData).Forget();
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            _lastPosition = eventData.position;
            CheckOverview();
        }

        private async UniTask CheckOverviewNextFrame(PointerEventData eventData)
        {
            await UniTask.NextFrame();

            DeactivateOverview();

            Debug.Log($"Input.mousePosition: {Input.mousePosition}; _lastPosition: {_lastPosition}");
            eventData.position = Input.mousePosition;

            //List<RaycastResult> results = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(eventData, results);

            //if (results.Count > 0)
            //{
            //    foreach (RaycastResult result in results)
            //    {
            //        Debug.Log();
            //    }
            //}
            if (EventSystem.current.TryGetFirstComponentInRaycasts<HistoryCardWatcher>(eventData, out HistoryCardWatcher findedComponent))
            {
                if (findedComponent == this)
                {
                    //Debug.Log("<color=#00A107>НАЙДЕН</color>, вызываем CheckOverview()");
                    CheckOverview();
                }
                //else
                //{
                //    Debug.Log("<color=#0000FF>НАЙДЕН, НО НЕ ОН</color>, не вызываем CheckOverview()");
                //}
            }
            //else
            //{
            //    Debug.Log("<color=#FF0000>НЕ НАЙДЕН</color>, не вызываем CheckOverview()");
            //}
        }

        private void CheckOverview()
        {
            // Важно: для UI используйте null в качестве камеры
            Debug.Log($"eventData.position: {_lastPosition}");

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_historyText, _lastPosition, null);

            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _historyText.textInfo.linkInfo[linkIndex];
                string linkInfoString = linkInfo.GetLinkID();

                CardViewData cardViewData = _historyCardLinkHandler.GetDataByLink(linkInfoString);

                //Если данных нет, нечего и пытаться их выводить
                if (cardViewData == null)
                    return;

                BigCardShowData bigCardShowData = new BigCardShowData(GameSettings.CardSize, _historyMenuRORTransform, cardViewData, BigCardPositionType.LeftOfTarget);
                BigCardRootActivateData bigCardRootActivateData = new BigCardRootActivateData(bigCardShowData);

                ActivateOverview(bigCardRootActivateData);
            }
            else
            {
                // Курсор не над ссылкой — скрываем карту
                DeactivateOverview();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DeactivateOverview();
        }

        public void DeactivateOverview()
        {
            //Debug.Log("TRY DeactivateOverview");

            if (_isActive == false)
            {
                //Debug.Log("NOT PERMITTED DeactivateOverview");
                return;
            }

            //Debug.Log("PERMITTED DeactivateOverview");

            _cardViewService.SetDefaultView();
            _isActive = false;

            //Debug.Log("SUCCESS DeactivateOverview");
        }

        private void ActivateOverview(BigCardRootActivateData bigCardRootActivateData)
        {
            //if (_isActive == true)
            //    return;

            //Debug.Log("TRY ActivateOverview");
            //Debug.Log("PERMITTED ActivateOverview");

            _cardViewService.SetOverview(null, bigCardRootActivateData, null);
            _isActive = true;
            //Debug.Log("SUCCESS ActivateOverview");
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(HistoryCardWatcher))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTextMeshProUGUI()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTextMeshProUGUI))]
        private ComponentAttachInfo DefineTextMeshProUGUI()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _historyText, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}