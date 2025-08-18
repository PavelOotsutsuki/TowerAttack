using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tools.Utils
{
    public static class Utils
    {
        public static List<T> Shuffle<T>(IEnumerable<T> targets)
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

        public static List<T> GetFlags<T>(T number) where T : Enum
        {
            List<T> result = new List<T>();
            int numberInt = Convert.ToInt32(number);

            for (int i = 1; i <= numberInt; i <<= 1)
            {
                if ((numberInt & i) == i)
                {
                    result.Add((T)(object)i);
                }
            }

            return result;
        }
    }
}