using GameFields.Histories;
using Servers;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class FirePoolEnemy : FirePool, IEnemyAIObject
    {
        public FirePoolEnemy(Transform parent, ExtraFireSeatActionRoot extraFireSeatActionRoot, HistoryRoot historyRoot, FightProcessDBManager fightProcessDBManager)
            : base(parent, extraFireSeatActionRoot, historyRoot, fightProcessDBManager)
        { }

        protected override string GetName() => nameof(FirePoolEnemy);
    }
}