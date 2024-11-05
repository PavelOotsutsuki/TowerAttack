using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AttackMenu : MonoBehaviour, IWorkable
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;
        [SerializeField] private AttackButton _attackButton;
        [SerializeField] private AttackNumberPanel _attackNumberPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        public void Init()
        {
            gameObject.SetActive(false);
            _canvasGroup.blocksRaycasts = false;

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            _attackButton.Init(Deactivate);
            _attackNumberPanel.Init(_attackButton);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _canvasGroup.blocksRaycasts = false;

            FadableLabelActivateData labelData = new FadableLabelActivateData("Выберете кого атакуем");
            _attackMenuLabel.Show(labelData);
            _attackMenuPanel.Activate();
            //_attackButton.Activate();
            _attackNumberPanel.Activate();


        }

        public void Deactivate()
        {
            _canvasGroup.blocksRaycasts = true;

            _attackMenuLabel.Hide();
            _attackNumberPanel.Deactivate();
            _attackButton.Deactivate();
            _attackMenuPanel.Deactivate();

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _attackMenuPanel.IsComplete && _attackMenuLabel.IsComplete && _attackButton.IsComplete && _attackNumberPanel.IsComplete);

            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAttackMenuLabel();
            DefineAttackMenuPanel();
            DefineAttackButton();
            DefineAttackNumberPanel();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineAttackMenuLabel))]
        private void DefineAttackMenuLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackMenuPanel))]
        private void DefineAttackMenuPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackButton))]
        private void DefineAttackButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private void DefineAttackNumberPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}