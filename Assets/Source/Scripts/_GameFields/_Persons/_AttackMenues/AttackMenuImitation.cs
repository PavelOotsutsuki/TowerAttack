using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AttackMenuImitation : MonoBehaviour, IAttackMenu, IActivatable, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;
        //[SerializeField] private AttackButton _attackButton;
        [SerializeField] private AttackNumberPanelEnemyAI _attackNumberPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private IAttackResultHandler _attackResultHandler;
        private AttackResult _attackResult;

        private int _countNumbers;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; private set; }

        public void Init(ICardNumberKeeper cardNumberKeeper, IAttackResultHandler attackResultHandler, int countNumbers)
        {
            gameObject.SetActive(false);
            IsComplete = false;
            _canvasGroup.blocksRaycasts = false;

            _countNumbers = countNumbers;

            _attackResultHandler = attackResultHandler;
            _attackResult = null;

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            //_attackButton.Init(this);
            //_attackNumberPanel.Init(_attackButton, cardNumberKeeper);
            _attackNumberPanel.Init(cardNumberKeeper, _countNumbers);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsComplete = false;
            IsActive = true;

            gameObject.SetActive(true);
            _canvasGroup.blocksRaycasts = true;

            FadableLabelActivateData labelData = new FadableLabelActivateData("Ожидаем противника...");
            _attackMenuLabel.Show(labelData);
            _attackMenuPanel.Show();

            _attackResult = new AttackResult();

            AttackNumberPanelActivateData numberPanelActivateData = new AttackNumberPanelActivateData(1, _attackResult);
            _attackNumberPanel.Activate(numberPanelActivateData);

            Deactivating().ToUniTask();
        }

        //public void Deactivate()
        //{
        //    if (IsActive == false)
        //        return;

        //    IsActive = false;

        //    //_canvasGroup.blocksRaycasts = false;

        //    Deactivating().ToUniTask();
        //}

        //private IEnumerator Attacking()
        //{
        //    //_attackButton.Deactivate();
        //    //_attackNumberPanel.Deactivate();

        //    yield return new WaitUntil(() => _attackNumberPanel.IsComplete);

        //    _attackMenuLabel.Hide();
        //    //_attackMenuPanel.Hide();

        //    //yield return new WaitUntil(() => _attackMenuPanel.IsComplete && _attackMenuLabel.IsComplete && _attackButton.IsComplete && _attackNumberPanel.IsComplete);
        //    yield return new WaitForSeconds(0.1f);
        //    yield return new WaitUntil(() => _attackMenuLabel.IsComplete);

        //    gameObject.SetActive(false);

        //    if (_attackResult.IsAttackSuccess)
        //    {
        //        _attackResultHandler.SuccessAttack();
        //        IsComplete = true;
        //    }
        //    else
        //    {
        //        _attackResultHandler.FalledAttack();
        //        IsComplete = true;
        //    }

        //    IsActive = false;
        //}

        private IEnumerator Deactivating()
        {
            //_attackButton.Deactivate();
            //_attackNumberPanel.Deactivate();




            yield return new WaitUntil(() => _attackNumberPanel.IsComplete);
            yield return new WaitForSeconds(1f);

            _attackMenuLabel.Hide();
            _attackMenuPanel.Hide();
            _attackNumberPanel.Deactivate();

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => _attackMenuPanel.IsComplete && _attackMenuLabel.IsComplete && _attackNumberPanel.IsComplete);

            gameObject.SetActive(false);

            if (_attackResult.IsAttackSuccess)
            {
                _attackResultHandler.SuccessAttack();
                IsComplete = true;
            }
            else
            {
                _attackResultHandler.FalledAttack();
                IsComplete = true;
            }

            IsActive = false;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(IAttackMenu))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAttackMenuLabel(),
                DefineAttackNumberPanel(),
                DefineCanvasGroup(),
                DefineAttackMenuPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAttackMenuLabel))]
        private ComponentAttachInfo DefineAttackMenuLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private ComponentAttachInfo DefineAttackNumberPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineAttackMenuPanel))]
        private ComponentAttachInfo DefineAttackMenuPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackMenuPanel, ComponentLocationTypes.InChildren);
        }

        #endregion 
    }
}