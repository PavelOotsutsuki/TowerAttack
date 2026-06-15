using System.Threading;
using Cards.DependencyInterlayers;
using Cysharp.Threading.Tasks;
using Tools.Settings;

namespace GameFields.Persons.Hands
{
    public class HandTransferZone : ExtraEffectZone, IHandTransferable
    {
        protected override string GetHistoryMsg()
        {
            return "Передача: ";
        }

        protected override void OnEndProcessing(CancellationToken token)
        {
            WaitingUntilComplete(token).Forget();
        }

        private async UniTask WaitingUntilComplete(CancellationToken token)
        {
            await UniTask.WaitForSeconds(GameSettings.DefaultEffectDelayBeforeComplete, cancellationToken: token);

            IsComplete = true;
        }
    }
}