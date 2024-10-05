using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.StartFights
{
    [Serializable]
    public class StartTowerCardSelectionImitationData
    {
        [SerializeField] private float _minWaitDurationBeforeStartActions = 6f;
        [SerializeField] private float _maxWaitDurationBeforeStartActions = 12f;

        public float WaitDurationBeforeStartActions => Random.Range(_minWaitDurationBeforeStartActions, _maxWaitDurationBeforeStartActions);
    }
}
