using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

namespace Tools.Extensions
{
    public static class DOTweenExtensions
    {
        public static async UniTask ToUniTask(this Tween tween, bool isCancelInvokeException = true, CancellationToken ct = default, Action onComplete = null)
        {
            AutoResetUniTaskCompletionSource tcs = AutoResetUniTaskCompletionSource.Create();

            void OnComplete()
            {
                onComplete?.Invoke();
                tcs.TrySetResult();
            };

            tween.OnComplete(OnComplete);
            tween.OnKill(() =>
            {
                //Debug.Log("OnKill DOTWeen from UniTask");
                if (ct.IsCancellationRequested && isCancelInvokeException)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult();
                }
            });

            using (ct.Register(() => tween.Kill()))
            {
                await tcs.Task;
            }
        }
    }
}