using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.AttackMenues
{
    public class AttackButton : ConfirmableButton, IActivatable
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
            gameObject.SetActive(true);
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

            gameObject.SetActive(false);
        }
    }
}