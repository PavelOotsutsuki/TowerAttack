using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.Decks;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using Servers;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameFields.EndFights
{
    public class ExitFightMenu : MonoBehaviour, IActivatable<ExitFightMenuActivateData>//, IPointerClickHandler
    {
        //private SwitchRootPanel _startEndGamePanel;
        [Inject] private DBRoot _dBRoot;

        private bool IsActive { get; set; } = false;

        private Action _onDestroyPrefab;
        private ExitFightMenuActivateData _data;

        //[Inject]
        //private void Construct(SwitchRootPanel startEndGamePanel)
        //{
        //    _startEndGamePanel = startEndGamePanel;
        //}

        public void Init(Action onDestroyPrefab)
        {
            _onDestroyPrefab = onDestroyPrefab;

            Deactivate();
        }

        public void Update()
        {
            if (Input.anyKeyDown)
            {
                Deactivating(this.destroyCancellationToken).Forget();
            }
        }

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    StartCoroutine(Ending());
        //}

        public void Activate(ExitFightMenuActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;
            _data = data;

            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            if (IsActive == false)
                return;

            IsActive = false;

            Deactivate();

            await _dBRoot.FinishFightWithBot(_data.IsWin, token);

            _onDestroyPrefab?.Invoke();
        }


        //private IEnumerator Ending()
        //{
        //    _startEndGamePanel.Show();

        //    yield return new WaitUntil(() => _startEndGamePanel.IsComplete);

        //    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //    _onDestroyPrefab?.Invoke();
        //    //SceneManager.LoadScene("StartMenu");
        //    //Application.Quit();
        //}
    }
}