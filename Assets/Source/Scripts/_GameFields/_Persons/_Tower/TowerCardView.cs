using Cards;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerCardView : MonoBehaviour, IWorkable<BigCardShowData>, IAutomaticFillComponents
    {
        [SerializeField] private TowerCardViewPanel _viewPanel;
        [SerializeField] private TowerBigCard _bigCard;

        private Coroutine _deactivateCoroutine = null; 
        private Coroutine _activateCoroutine = null;

        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _viewPanel.Init();
            _bigCard.Init();

            gameObject.SetActive(false);
        }

        public void Activate(BigCardShowData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            gameObject.SetActive(true);

            _activateCoroutine = StartCoroutine(Activating(data));
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            if (_activateCoroutine != null)
            {
                StopCoroutine(_activateCoroutine);
                _activateCoroutine = null;
            }

            _viewPanel.Hide();
            _bigCard.Hide();

            _deactivateCoroutine = StartCoroutine(WaitUntilSetDeactivate());
        }

        private void OnDisable()
        {
            _deactivateCoroutine = null;
            _activateCoroutine = null;
        }

        private IEnumerator Activating(BigCardShowData data)
        {
            if (_deactivateCoroutine != null)
            {
                StopCoroutine(_deactivateCoroutine);
                _deactivateCoroutine = null;
            }
            else
            {
                yield return new WaitForSeconds(0.8f);
            }

            _viewPanel.Show();
            _bigCard.Show(data);
        }

        private IEnumerator WaitUntilSetDeactivate()
        {
            yield return new WaitUntil(() => _viewPanel.IsComplete && _bigCard.IsComplete);

            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerCardView))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerCardViewPanel(),
                DefineTowerBigCard()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerCardViewPanel))]
        private ComponentAttachInfo DefineTowerCardViewPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _viewPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTowerBigCard))]
        private ComponentAttachInfo DefineTowerBigCard()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}