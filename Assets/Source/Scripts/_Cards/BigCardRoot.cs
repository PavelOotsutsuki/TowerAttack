using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Cards
{
    [RequireComponent(typeof(FadablePanel))]
    public class BigCardRoot : MonoBehaviour, IWorkable<BigCardRootActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private BigCardDescription _description;
        [SerializeField] private CardDescription _cardDescription;

        private CancellationTokenSource _tokenSource;
        private UniTask _deactivating;

        private CardCapabilityDescription _cardCapabilityDescription;

        public bool? IsActive { get; private set; } = null;

        public void Init(CardCapabilityDescription cardCapabilityDescription)
        {
            _cardCapabilityDescription = cardCapabilityDescription;

            _bigCard.Init(_cardCapabilityDescription);
            _description.Init();
            _cardDescription.Init();
        }

        public void Activate(BigCardRootActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            CancelDeactivating();
            SetPosition(data.ViewType);
            //_transform.position = data.Position;

            //_activatingBigCard = ActivatingBigCard(data.BigCardShowData, data.BigCardActivateDelay)
            //    .ToUniTask(cancellationToken: _token.Token);
            _bigCard.Show(data.BigCardShowData);

            _cardDescription.Show(new LabelActivateData(data.BigCardShowData.CardViewData.Description));

            //string cardCapabilityDescription = _cardCapabilityDescription.GetDescription(data.BigCardShowData.CardViewData.CardCapability);
            string cardCapabilityDescription = _cardCapabilityDescription.GetAllCapabilitiesToStringValue(data.BigCardShowData.CardViewData.CardCapability);

            if (cardCapabilityDescription != "")
            {
                LabelActivateData labelActivateData = new LabelActivateData(cardCapabilityDescription);
                //_activatingBigCardDescription = ActivatingBigCardDescription(labelActivateData,
                //    data.BigCardDescriptionActivateDelay).ToUniTask(cancellationToken: _token.Token);
                _description.Show(labelActivateData);
            }

            _fadablePanel.Show();
        }

        private void SetPosition(BigCardViewType viewType)
        {
            switch (viewType)
            {
                case BigCardViewType.LeftTop:
                    SetLeftTopPosition();
                    break;
                case BigCardViewType.RightTop:
                    SetRightTopPosition();
                    break;
                case BigCardViewType.AroundTarget:
                    SetAroundTargetPosition();
                    break;
                default:
                    throw new Exception("Неизвестный viewType: " + viewType);
            }
        }

        private void SetLeftTopPosition()
        {
            Debug.Log("SetLeftTopPosition");

            _rectTransform.anchorMin = new Vector2(0f, 1f);
            _rectTransform.anchorMax = new Vector2(0f, 1f);
            _rectTransform.pivot = new Vector2(0f, 1f);

            _rectTransform.anchoredPosition = Vector2.zero;
            ((RectTransform)_bigCard.transform).anchoredPosition = new Vector2(-194f, -2f);
            ((RectTransform)_description.transform).anchoredPosition = new Vector2(257f, 110f);
        }

        private void SetRightTopPosition()
        {
            Debug.Log("SetRightTopPosition");

            _rectTransform.anchorMin = new Vector2(1f, 1f);
            _rectTransform.anchorMax = new Vector2(1f, 1f);
            _rectTransform.pivot = new Vector2(1f, 1f);

            _rectTransform.anchoredPosition = Vector2.zero;
            ((RectTransform)_bigCard.transform).anchoredPosition = new Vector2(194f, -2f);
            ((RectTransform)_description.transform).anchoredPosition = new Vector2(-267f, 110f);
        }

        private void SetAroundTargetPosition()
        {
            Debug.Log("SetAroundTargetPosition");

            _rectTransform.anchorMin = new Vector2(0f, 1f);
            _rectTransform.anchorMax = new Vector2(0f, 1f);
            _rectTransform.pivot = new Vector2(0f, 1f);

            _rectTransform.anchoredPosition = Vector2.zero;
            ((RectTransform)_bigCard.transform).anchoredPosition = new Vector2(-194f, -2f);
            ((RectTransform)_description.transform).anchoredPosition = new Vector2(257f, 110f);
        }

        //private IEnumerator ActivatingBigCard(BigCardShowData data, float delay)
        //{
        //    delay = 0f;

        //    if (Mathf.Approximately(delay, 0f) == false)
        //        yield return new WaitForSeconds(delay);

        //    _bigCard.Show(data);
        //}

        //private IEnumerator ActivatingBigCardDescription(LabelActivateData data, float delay)
        //{
        //    delay = 0f;

        //    if (Mathf.Approximately(delay, 0f) == false)
        //        yield return new WaitForSeconds(delay);

        //    _description.Show(data);
        //}

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            //_bigCard.Hide();
            //_description.Hide();
            //_cardDescription.Hide();
            //_fadablePanel.Hide();

            _tokenSource = new CancellationTokenSource();
            _deactivating = Deacitvating().ToUniTask(cancellationToken: _tokenSource.Token);
        }

        private IEnumerator Deacitvating()
        {
            _fadablePanel.Hide();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _bigCard.Hide();
            _description.Hide();
            _cardDescription.Hide();
        }

        private void CancelDeactivating()
        {
            if (_deactivating.Status == UniTaskStatus.Pending)
            {
                _tokenSource.Cancel();
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(BigCardRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineFadablePanel(),
                DefineBigCard(),
                DefineBigCardDescription(),
                DefineCardDescription()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineBigCard))]
        private ComponentAttachInfo DefineBigCard()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineBigCardDescription))]
        private ComponentAttachInfo DefineBigCardDescription()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _description, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardDescription))]
        private ComponentAttachInfo DefineCardDescription()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}