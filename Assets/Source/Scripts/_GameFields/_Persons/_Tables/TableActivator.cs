using System.Collections;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TableActivator : MonoBehaviour, ITableActivator, ITableDeactivator
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _activateDelay = 0.5f;

        private Coroutine _inWork;

        public void Activate()
        {
            _inWork = StartCoroutine(Activating());
        }

        public void Deactivate()
        {
            StartCoroutine(Deactivating());
        }

        private IEnumerator Activating()
        {
            yield return new WaitForSeconds(_activateDelay);

            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            _canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(()=> _inWork is not null);

            _canvasGroup.blocksRaycasts = false;
        }
    }
}
