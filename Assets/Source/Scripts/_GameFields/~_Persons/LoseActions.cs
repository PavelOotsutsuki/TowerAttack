using System;
using System.Threading;
using Cards.Effects;
using Cysharp.Threading.Tasks;
using GameFields.EndFights;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Servers;
using Tools;
using Zenject;

namespace GameFields.Persons
{
    public class LoseActions: IActivatable<CancellationTokenData>
    {
        private bool _isActive;

        private readonly IBoomTower _boomedTower;
        private readonly IPersonObject _loser;
        private readonly IHandBlockable _hand;
        private readonly SignalBus _bus;
        private readonly IDeactivatable _inputRoot;
        private readonly IDeactivatable _fightMenu;
        private readonly IDeactivatable _fightButtonsActivator;
        private readonly ISoundController _soundController;
        private readonly DBRoot _dBRoot;

        public LoseActions(IBoomTower boomedTower, IPersonObject loser, IHandBlockable handBlockable, SignalBus bus,
            IDeactivatable inputRoot, IDeactivatable fightMenu, IDeactivatable fightButtonsActivator, ISoundController soundController,
            DBRoot dBRoot)
        {
            _boomedTower = boomedTower;
            _loser = loser;
            _hand = handBlockable;
            _bus = bus;
            _inputRoot = inputRoot;
            _fightMenu = fightMenu;
            _fightButtonsActivator = fightButtonsActivator;
            _soundController = soundController;
            _dBRoot = dBRoot;

            _isActive = false;
        }

        public void Activate(CancellationTokenData data)
        {
            if (_isActive)
                return;

            _isActive = true;

            Activating(data.Token).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            _inputRoot.Deactivate();
            _fightMenu.Deactivate();
            _fightButtonsActivator.Deactivate();
            _hand.ForciblyBlock();
            _boomedTower.Boom();
            _soundController.Stop();

            DateTime before = DateTime.UtcNow;
            bool? result = _loser switch
            {
                IEnemyAIObject => true,
                IPlayerObject => false,
                _ => null
            };
            await _dBRoot.FinishFightWithBot(result, token);

            int timeAfterMilliseconds = Convert.ToInt32((DateTime.UtcNow - before).TotalMilliseconds);

            if (5000 - timeAfterMilliseconds > 0)
                await UniTask.Delay(5000 - timeAfterMilliseconds, cancellationToken: token);

            _bus.Fire(new PersonWinSignal(_loser));
        }
    }
}