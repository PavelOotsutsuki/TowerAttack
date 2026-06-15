using System.Threading;
using Cards.DependencyInterlayers;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Histories;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.Hands;
using Tools.Settings;
using Zenject;

namespace GameFields.DiscardPiles
{
    public class ForgingZone : ExtraEffectZone, IForging
    {
        private IDrawCardManager _drawCardManager;
        private GnomeEffectHandler _gnomeEffectHandler;

        public void Init(ICardSeatable cardSeatable, SignalBus signalBus, IDrawCardManager drawCardManager,
            GnomeEffectHandler gnomeEffectHandler, HistoryRoot historyRoot)
        {
            base.Init(cardSeatable, signalBus, historyRoot);

            _drawCardManager = drawCardManager;
            _gnomeEffectHandler = gnomeEffectHandler;
        }

        protected override void OnEndProcessing(CancellationToken token)
        {
            _drawCardManager.DrawCards(1, token, () => Continue(token));
            _gnomeEffectHandler.Upgrade();
        }

        private async UniTask WaitingUntilComplete(CancellationToken token)
        {
            await UniTask.WaitForSeconds(GameSettings.DefaultEffectDelayBeforeComplete, cancellationToken: token);

            IsComplete = true;
        }

        private void Continue(CancellationToken token)
        {
            WaitingUntilComplete(token).Forget();
        }

        protected override string GetHistoryMsg()
        {
            return "Гномичья ковка: ";
        }
    }
}