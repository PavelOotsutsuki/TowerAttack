using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Towers;
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

            _onPointerClick?.Invoke();
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