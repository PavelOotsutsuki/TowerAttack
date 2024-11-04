using System.Collections;
using Cysharp.Threading.Tasks;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackButton : FadableConfirmableButton
    {
        public override void Activate()
        {
            base.Activate();

            gameObject.SetActive(true);
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