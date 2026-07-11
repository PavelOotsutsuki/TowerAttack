using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tools.UI;
using UnityEditor;
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

        public static void Quit()
        {
            #if UNITY_EDITOR
            {
                EditorApplication.isPlaying = false;
            }
            #else
            {
                Application.Quit();
            }
            #endif
        }

        public static void DestroyCTS(ref CancellationTokenSource cts, [CallerMemberName] string callerMethod = "",
                     [CallerFilePath] string callerFile = "",
                     [CallerLineNumber] int callerLine = 0)
        {
            if (cts == null)
                return;

            //Debug.Log($"DestroyCTS: {callerFile.Substring(callerFile.LastIndexOf('/') + 1).Replace(".cs", "")}.{callerMethod}, {callerLine}");

            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }

        public static void PrintGCInfo()
        {
            int gen0Collections = GC.CollectionCount(0);
            int gen1Collections = GC.CollectionCount(1);
            int gen2Collections = GC.CollectionCount(2);

            Debug.Log($"<b>Количество сборок:</b> Gen0={gen0Collections}, Gen1={gen1Collections}, Gen2={gen2Collections}");
        }


        //public static async UniTask DoAnimationAsync(CancellationToken ct, Tween tween)
        //{
        //    AutoResetUniTaskCompletionSource tcs = AutoResetUniTaskCompletionSource.Create();

        //    // Подписываемся на завершение твина
        //    tween.OnComplete(() => tcs.TrySetResult());
        //    tween.OnKill(() => tcs.TrySetResult());

        //    // При отмене токена — убиваем твин
        //    using (ct.Register(() => tween.Kill()))
        //    {
        //        await tcs.Task;
        //    }
        //}

        //public static async UniTask CancelledExecute(CancellationToken mainToken, string className, Func<CancellationToken, UniTask> asyncAction, string methodName, CancellationToken? localToken = null)
        //{
        //    if (mainToken.IsCancellationRequested)
        //        return;

        //    localToken ??= mainToken;

        //    try
        //    {
        //        await asyncAction(localToken.Value);
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Debug.Log($"ОТМЕНА ТОКЕНА: {methodName}: {className}");
        //    }
        //}

        //public static async UniTask<T> CancelledExecute<T>(CancellationToken mainToken, string className, Func<CancellationToken, UniTask<T>> asyncAction, string methodName, CancellationToken? localToken = null)
        //{
        //    if (mainToken.IsCancellationRequested)
        //        return default;

        //    localToken ??= mainToken;

        //    try
        //    {
        //        return await asyncAction(localToken.Value);
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Debug.Log($"ОТМЕНА ТОКЕНА: {methodName}: {className}");
        //        return default;
        //    }
        //}

        //public static async Task WrapWithCancellation(CancellationToken token,
        //Func<Task> asyncAction,
        //string callerName = null)
        //{
        //    if (token.IsCancellationRequested)
        //        return;

        //    try
        //    {
        //        await asyncAction();
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Debug.Log($"ОТМЕНА ТОКЕНА: {callerName ?? "Unknown"}");
        //    }
        //}

        //// Версия с возвращаемым значением
        //public static async Task<T> WrapWithCancellation<T>(
        //    CancellationToken token,
        //    Func<Task<T>> asyncAction,
        //    string callerName = null)
        //{
        //    if (token.IsCancellationRequested)
        //        return default;

        //    try
        //    {
        //        return await asyncAction();
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Debug.Log($"ОТМЕНА ТОКЕНА: {callerName ?? "Unknown"}");
        //        return default;
        //    }
        //}
    }
}