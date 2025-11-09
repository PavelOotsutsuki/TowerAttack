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
    public class BigCardRoot : MonoBehaviour, IWorkable<BigCardRootActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private BigCardDescription _description;
        [SerializeField] private CardDescription _cardDescription;

        private CardCapabilityDescription _cardCapabilityDescription;

        private CancellationTokenSource _token;
        private UniTask _activatingBigCard;
        private UniTask _activatingBigCardDescription;

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

            CancelActivating();

            _token = new CancellationTokenSource();
            _activatingBigCard = ActivatingBigCard(data.BigCardShowData, data.BigCardActivateDelay)
                .ToUniTask(cancellationToken: _token.Token);

            _cardDescription.Show(data.BigCardShowData.LabelData);

            //string cardCapabilityDescription = _cardCapabilityDescription.GetDescription(data.BigCardShowData.CardViewData.CardCapability);
            string cardCapabilityDescription = _cardCapabilityDescription.GetAllCapabilitiesToStringValue(data.BigCardShowData.CardViewData.CardCapability);

            if (cardCapabilityDescription != "")
            {
                LabelActivateData labelActivateData = new LabelActivateData(cardCapabilityDescription);
                _activatingBigCardDescription = ActivatingBigCardDescription(labelActivateData,
                    data.BigCardDescriptionActivateDelay).ToUniTask(cancellationToken: _token.Token);
            }
        }

        private IEnumerator ActivatingBigCard(BigCardShowData data, float delay)
        {
            if (Mathf.Approximately(delay, 0f) == false)
                yield return new WaitForSeconds(delay);

            _bigCard.Show(data);
        }

        private IEnumerator ActivatingBigCardDescription(LabelActivateData data, float delay)
        {
            if (Mathf.Approximately(delay, 0f) == false)
                yield return new WaitForSeconds(delay);

            _description.Show(data);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            CancelActivating();

            _bigCard.Hide();
            _description.Hide();
            _cardDescription.Hide();
        }

        private void CancelActivating()
        {
            if (_activatingBigCard.Status == UniTaskStatus.Pending ||
                _activatingBigCardDescription.Status == UniTaskStatus.Pending)
            {
                _token.Cancel();
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(BigCardRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineBigCard(),
                DefineBigCardDescription()
            };

            return list;
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
        #endregion
    }
}
