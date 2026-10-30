using System;
using System.Runtime;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields
{
    public static class GameFieldGC
    {
        private const int MODE = 1;

        private static DateTime _lastCollect = DateTime.Now;
        // 1 - коллектор работает как обычно
        // 2 - ручной вызов коллектора
        // 3 - SustainedLowLatency + ручной вызов коллектора

        public static void GCOFF(CancellationToken gameFieldToken)
        {
            if (MODE == 3)
            {
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
                Logging(gameFieldToken).Forget();
            }
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
                LogCollect();
                //GC.Collect();
                //_lastCollect = DateTime.Now;
                Debug.Log("Обычный GC.Collect()");
                return;
            }
        }

        //public static void CollectIfNeeded()
        //{
        //    if (MODE != 1)
        //    {
        //        DateTime now = DateTime.Now;
        //        //Debug.Log($"now={now};_lastCollect={_lastCollect};(now - _lastCollect).Seconds");
        //        if ((now - _lastCollect).Minutes > 2)
        //        {
        //            LogCollect();
        //            //GC.Collect();
        //            //_lastCollect = now;
        //            Debug.Log("CollectIfNeeded()");
        //            return;
        //        }
        //    }
        //}

        private static async UniTask Logging(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                int secondsToAwait = 120 - Convert.ToInt32((DateTime.Now - _lastCollect).TotalSeconds);

                if (secondsToAwait <= 0)
                {
                    Debug.Log($"Странное secondsToAwait: {secondsToAwait}");
                    secondsToAwait = 1;
                }

                await UniTask.WaitForSeconds(secondsToAwait + 1, cancellationToken: token);

                Debug.Log($"secondsToAwait={secondsToAwait}; now={DateTime.Now.TimeOfDay}; _lastCollect={_lastCollect.TimeOfDay}");
                if (Convert.ToInt32((DateTime.Now - _lastCollect).TotalSeconds) > 120)
                {
                    LogCollect();
                }
            }
        }

        private static void LogCollect()
        {
            // ── Способ 2: Принудительная сборка и сравнение ──
            // Собираем только Gen 0
            long memBefore = GC.GetTotalMemory(false);
            GC.Collect(0);
            long memAfterGen0 = GC.GetTotalMemory(false);
            long gen0Size = memBefore - memAfterGen0;

            Debug.Log($"<b>Gen 0 (приблизительно):</b> {gen0Size / 1024f:F2} KB (освобождено)");

            // Собираем Gen 0 и Gen 1
            GC.Collect(1);
            long memAfterGen1 = GC.GetTotalMemory(false);
            long gen1Size = memAfterGen0 - memAfterGen1;

            Debug.Log($"<b>Gen 1 (приблизительно):</b> {gen1Size / 1024f:F2} KB (освобождено)");

            // Полная сборка (Gen 0, 1 и 2 + LOH)
            GC.Collect();
            long memAfterGen2 = GC.GetTotalMemory(false);
            long gen2Size = memAfterGen1 - memAfterGen2;

            Debug.Log($"<b>Gen 2 + LOH (приблизительно):</b> {gen2Size / 1024f:F2} KB (освобождено)");
            Debug.Log($"<b>Осталось после Full GC:</b> {memAfterGen2 / 1024f:F2} KB");

            _lastCollect = DateTime.Now;
        }
    }
}