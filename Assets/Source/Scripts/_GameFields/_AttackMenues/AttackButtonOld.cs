using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.AttackMenues
{
    public class AttackButtonOld : MonoBehaviour, IActivatable
    {
        [SerializeField] private Button _button;
        [SerializeField] private FadablePanel _fadablePanel;

        private AttackNumberPanel _attackNumberPanel;

        public void Init(AttackNumberPanel attackNumberPanel)
        {
            _fadablePanel.Init();
            _attackNumberPanel = attackNumberPanel;

            gameObject.SetActive(false);
        }

        //public void Init()
        //{
        //    _fadablePanel.Init();

        //    Deactivate();
        //}

        public void Activate()
        {
            gameObject.SetActive(true);
            //_attackNumberPanel.Unsubscribe();
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
