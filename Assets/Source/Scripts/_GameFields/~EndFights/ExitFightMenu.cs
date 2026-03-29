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
        private StartEndGamePanel _startEndGamePanel;

        [Inject]
        private void Construct(StartEndGamePanel startEndGamePanel)
        {
            _startEndGamePanel = startEndGamePanel;
        }

        public void Init()
        {
            Deactivate();
        }

        public void Update()
        {
            if (Input.anyKeyDown)
            {
                Deactivate();
                Ending().ToUniTask();
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

        private IEnumerator Ending()
        {
            _startEndGamePanel.Show();

            yield return new WaitUntil(() => _startEndGamePanel.IsComplete);

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("StartMenu");
            //Application.Quit();
        }
    }
}