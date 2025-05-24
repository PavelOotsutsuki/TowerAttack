using System;
using UnityEngine;

namespace GameFields
{
    public static class GameFieldGC
    {
        private const float MaxPosiblePercentMemory = 0.8f;

        private static bool _isActive = false;

        private static long _startGCMemory;
        private static long _endGCMemory;
        private static long _maxSize;

        public static void StartRememberGCMemory()
        {
            _startGCMemory = GC.GetTotalMemory(true);
        }

        public static void EndRememberGCMemory()
        {
            _endGCMemory = GC.GetTotalMemory(true);
            _maxSize = _endGCMemory - _startGCMemory;
            Debug.Log($"_startGCMemory: {_startGCMemory}\n_endGCMemory: {_endGCMemory}\n_maxSize: {_maxSize}");
        }

        public static void StartWait()
        {
            if (GC.TryStartNoGCRegion((long)(_maxSize * MaxPosiblePercentMemory)))
            {
                Debug.Log("Память выделена");
                _isActive = true;
            }
            else
            {
                Debug.Log("GC отказался выделять память");
                _isActive = false;
            }
        }

        public static void Collect()
        {
            if (_isActive == false)
            {
                GC.Collect();
                Debug.Log("Обычный GC.Collect()");
                return;
            }

            GC.EndNoGCRegion();
            GC.Collect();

            StartWait();
            Debug.Log("Кастомный Collect()");
        }
    }
}