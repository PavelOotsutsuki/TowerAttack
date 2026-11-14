using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Discovers
{
    public class DiscoverAI : Discover, IEnemyAIObject
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
            WaitingToSelect(Token).Forget();
        }

        private async UniTask WaitingToSelect(CancellationToken token)
        {
            int selectedCardNumber = Random.Range(0, Cards.Count);
            float waitDuration = Random.Range(_minWaitDuration, _maxWaitDuration);

            await UniTask.WaitForSeconds(waitDuration, cancellationToken: token);

            Seats[selectedCardNumber].StartClick();
        }
    }
}