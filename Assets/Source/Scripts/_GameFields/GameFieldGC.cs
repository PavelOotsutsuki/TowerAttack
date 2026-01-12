using System;
using System.Runtime;
using UnityEngine;

namespace GameFields
{
    public static class GameFieldGC
    {
        private const int MODE = 3;
        // 1 - коллектор работает как обычно
        // 2 - ручной вызов коллектора
        // 3 - SustainedLowLatency + ручной вызов коллектора

        public static void GCOFF()
        {
            if (MODE == 3)
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }

        public static void GCON()
        {
            if (MODE == 3)
                GCSettings.LatencyMode = GCLatencyMode.Interactive;
        }

        public static void Collect()
        {
            if (MODE != 1)
            {
                GC.Collect();
                Debug.Log("Обычный GC.Collect()");
                return;
            }
        }
    }
}