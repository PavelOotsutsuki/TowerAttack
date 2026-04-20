using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.Decks;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameFields.EndFights
{
    public class ExitFightMenu : MonoBehaviour, IActivatable//, IPointerClickHandler
    {
        //private SwitchRootPanel _startEndGamePanel;
        private Action _onDestroyPrefab;

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
                Deactivate();
                _onDestroyPrefab?.Invoke();
            }
        }

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    StartCoroutine(Ending());
        //}

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
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