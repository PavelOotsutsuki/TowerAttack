using System.Collections;
using Tools;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectButton : FadableConfirmableButton
    {
        private IDeactivatable _clickCallback;
        private Coroutine _workableCoroutine;

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

            if (_workableCoroutine != null)
                StopCoroutine(_workableCoroutine);

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

            if (_workableCoroutine != null)
                StopCoroutine(_workableCoroutine);

            _workableCoroutine = StartCoroutine(Deactivating());
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => IsComplete);

            gameObject.SetActive(false);
        }
    }
}