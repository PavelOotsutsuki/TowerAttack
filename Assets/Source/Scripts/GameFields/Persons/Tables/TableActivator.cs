using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TableActivator : MonoBehaviour, ITableActivator, ITableDeactivator
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Activate()
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            _canvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
