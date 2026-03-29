using GameFields.Histories;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class FirePoolPlayer : FirePool, IPlayerObject
    {
        public FirePoolPlayer(Transform parent, ExtraFireSeatActionRoot extraFireSeatActionRoot, HistoryRoot historyRoot)
            : base(parent, extraFireSeatActionRoot, historyRoot)
        { }
    }
}