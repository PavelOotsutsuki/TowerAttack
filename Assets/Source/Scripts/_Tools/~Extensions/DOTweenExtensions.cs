using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;

namespace Tools.Extensions
{
    public static class DOTweenExtensions
    {
        public static async UniTask ToUniTask(this Tween tween, bool isCancelInvokeException = true, CancellationToken ct = default)
        {
            AutoResetUniTaskCompletionSource tcs = AutoResetUniTaskCompletionSource.Create();

            tween.OnComplete(() => tcs.TrySetResult());
            tween.OnKill(() =>
            {
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