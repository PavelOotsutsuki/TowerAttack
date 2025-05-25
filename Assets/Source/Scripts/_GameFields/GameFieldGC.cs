using System;
using System.Runtime;
using UnityEngine;

namespace GameFields
{
    public static class GameFieldGC
    {
        //private const float MaxPosiblePercentMemory = 0.8f;

        //private static bool _isActive = false;

        //private static long _startGCMemory;
        //private static long _endGCMemory;
        //private static long _maxSize;

        public static void GCOFF()
        {
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }

        public static void GCON()
        {
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
        }

        //public static void StartRememberGCMemory()
        //{
        //    _startGCMemory = GC.GetTotalMemory(true);
        //}

        //public static void EndRememberGCMemory()
        //{
        //    _endGCMemory = GC.GetTotalMemory(true);
        //    _maxSize = _endGCMemory - _startGCMemory;
        //    Debug.Log($"_startGCMemory: {_startGCMemory}\n_endGCMemory: {_endGCMemory}\n_maxSize: {_maxSize}");
        //}

        //public static void StartWait()
        //{
        //    GCSettings.LatencyMode

        //    if (GC.TryStartNoGCRegion(4505600))
        //    {
        //        Debug.Log("Память выделена");
        //        _isActive = true;
        //    }
        //    else
        //    {
        //        Debug.Log("GC отказался выделять память");
        //        _isActive = false;
        //    }
        //}

        //public static void Collect()
        //{
        //    if (_isActive == false)
        //    {
        //        GC.Collect();
        //        Debug.Log("Обычный GC.Collect()");
        //        return;
        //    }

        //    GC.EndNoGCRegion();
        //    GC.Collect();

        //    StartWait();
        //    Debug.Log("Кастомный Collect()");
        //}

        public static void Collect()
        {
            GC.Collect();
            Debug.Log("Обычный GC.Collect()");
            return;
        }
    }
}