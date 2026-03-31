using GameFields.Histories;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class FirePoolEnemy : FirePool, IEnemyAIObject
    {
        public FirePoolEnemy(Transform parent, ExtraFireSeatActionRoot extraFireSeatActionRoot, HistoryRoot historyRoot)
            : base(parent, extraFireSeatActionRoot, historyRoot)
        { }
    }
}