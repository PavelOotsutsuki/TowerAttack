using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    [RequireComponent(typeof(FadablePanel))]
    [RequireComponent(typeof(BigCard))]
    public class TowerBigCard : MonoBehaviour, IViewable<BigCardShowData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private FadablePanel _fadablePanel;

        private bool _isComplete;
        private Coroutine _hiddingCoroutine = null;

        public bool? IsShown => _fadablePanel.IsShown!= null && _bigCard.IsShown != null? _fadablePanel.IsShown.Value && _bigCard.IsShown.Value: null;
        public bool IsComplete => _isComplete;

        public void Init()
        {
            _isComplete = false;

            _bigCard.Init();
            _fadablePanel.Init();
        }

        public void Show(BigCardShowData data)
        {
            if (IsShown == true)
                return;

            if (_hiddingCoroutine != null)
                StopCoroutine(_hiddingCoroutine);

            _isComplete = false;

            _bigCard.Show(data);
            _fadablePanel.Show();
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            _hiddingCoroutine = StartCoroutine(Hidding());
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