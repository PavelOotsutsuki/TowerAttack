using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackButton : FadableConfirmableButton
    {
        private IDeactivatable _clickCallback;

        public void Init(IDeactivatable clickCallback)
        {
            _clickCallback = clickCallback;
            IsActive = false;

            base.Init();

            gameObject.SetActive(false);
        }

        public override void Activate()
        {
            if (IsActive == true)
                return;

            gameObject.SetActive(true);

            base.Activate();

            IsActive = true;
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _clickCallback.Deactivate();
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            base.Deactivate();

            IsActive = false;

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => IsComplete);

            gameObject.SetActive(false);
        }
    }
}