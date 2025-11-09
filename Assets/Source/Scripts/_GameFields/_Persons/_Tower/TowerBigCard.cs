using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Towers
{
    [RequireComponent(typeof(FadablePanel))]
    [RequireComponent(typeof(BigCard))]
    public class TowerBigCard : MonoBehaviour, IViewable<BigCardShowData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private FadablePanel _fadablePanel;

        private CardCapabilityDescription _cardCapabilityDescription;

        private bool _isComplete;
        private Coroutine _hiddingCoroutine = null;

        public bool? IsShown { get; private set; } = null;
        public bool IsComplete => _isComplete;

        [Inject]
        public void Construct(CardCapabilityDescription cardCapabilityDescription)
        {
            _cardCapabilityDescription = cardCapabilityDescription;
        }

        public void Init()
        {
            _isComplete = true;

            _bigCard.Init(_cardCapabilityDescription);
            _fadablePanel.Init();

            IsShown = false;
        }

        public void Show(BigCardShowData data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            if (_hiddingCoroutine != null)
            {
                StopCoroutine(_hiddingCoroutine);
                _hiddingCoroutine = null;
            }

            _isComplete = false;

            _bigCard.Show(data);
            _fadablePanel.Show();
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _hiddingCoroutine = StartCoroutine(Hidding());
        }

        private void OnDisable()
        {
            _hiddingCoroutine = null;
        }

        private IEnumerator Hidding()
        {
            _fadablePanel.Hide();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _bigCard.Hide();

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerBigCard))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineBigCard(),
                DefineFadablePanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineBigCard))]
        private ComponentAttachInfo DefineBigCard()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}