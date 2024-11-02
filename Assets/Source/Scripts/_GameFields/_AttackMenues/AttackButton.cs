using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI.Buttons;
using Tools.UI.Fadings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class AttackButton : ConfirmableButton, IWorkable
    {
        [SerializeField] private FadablePanel _fadablePanel;

        public override void Init()
        {
            base.Init();

            _fadablePanel.Init();

            gameObject.SetActive(false);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        public void Activate()
        {
            //gameObject.SetActive(true);
            _fadablePanel.Show();
        }

        public void Deactivate()
        {
            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            _fadablePanel.Hide();

            yield return new WaitUntil(() => _fadablePanel.IsComplete); 

            //gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        protected override void DefineAllComponents()
        {
            DefineFadablePanel();

            base.DefineAllComponents();
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private void DefineFadablePanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}