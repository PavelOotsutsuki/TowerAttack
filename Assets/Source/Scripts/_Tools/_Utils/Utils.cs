using System.Collections.Generic;
using UnityEngine;

namespace Tools.Utils
{
    public static class Utils
    {
        public static List<T> Shuffle<T>(IReadOnlyList<T> targets)
        {
            List<T> shuffleList = new List<T>();
            List<T> cloneTargets = new List<T>();

            foreach (T item in targets)
            {
                cloneTargets.Add(item);
            }

            while (cloneTargets.Count > 0)
            {
                T item = cloneTargets[Random.Range(0, cloneTargets.Count)];
                shuffleList.Add(item);
                cloneTargets.Remove(item);
            }

            return shuffleList;
        }
    }
}