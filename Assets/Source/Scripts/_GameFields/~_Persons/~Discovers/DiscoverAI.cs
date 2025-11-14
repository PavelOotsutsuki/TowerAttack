using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Discovers
{
    public class DiscoverAI : Discover
    {
        private const int CountLogics = 1;

        [SerializeField] private float _minWaitDuration = 1.5f;
        [SerializeField] private float _maxWaitDuration = 5f;

        public override void Activate(DiscoverActivateData data)
        {
            base.Activate(data);

            int logicNumber = Random.Range(1, CountLogics + 1);

            if (logicNumber == 1)
            {
                StartLogic1();
            }
        }

        private void StartLogic1()
        {
            WaitingToSelect().ToUniTask();
        }

        private IEnumerator WaitingToSelect()
        {
            int selectedCardNumber = Random.Range(0, Cards.Count);
            float waitDuration = Random.Range(_minWaitDuration, _maxWaitDuration);

            yield return new WaitForSeconds(waitDuration);

            Seats[selectedCardNumber].StartClick();
        }
    }
}