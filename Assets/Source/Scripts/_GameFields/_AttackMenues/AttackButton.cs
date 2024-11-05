using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackButton : FadableConfirmableButton
    {
        private Action _onPointerClick;

        public void Init(Action onPointerClick)
        {
            _onPointerClick = onPointerClick;

            base.Init();

            gameObject.SetActive(false);
        }

        public override void Activate()
        {
            gameObject.SetActive(true);

            base.Activate();
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _onPointerClick?.Invoke();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => IsComplete);

            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        protected override void DefineAllComponents()
        {
            base.DefineAllComponents();
        }
        #endregion 
    }
}