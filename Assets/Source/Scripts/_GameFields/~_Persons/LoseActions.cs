using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.FightMenues;
using GameFields.InputSettings;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public LoseActions(IBoomTower boomedTower, IPersonObject loser, IHandBlockable handBlockable, SignalBus bus,
            IDeactivatable inputRoot, IDeactivatable fightMenu, IDeactivatable fightButtonsActivator, ISoundController soundController)
        {
            _boomedTower = boomedTower;
            _loser = loser;
            _hand = handBlockable;
            _bus = bus;
            _inputRoot = inputRoot;
            _fightMenu = fightMenu;
            _fightButtonsActivator = fightButtonsActivator;
            _soundController = soundController;

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

            await UniTask.Delay(5000, cancellationToken: token);

            _bus.Fire(new PersonWinSignal(_loser));
        }
    }
}