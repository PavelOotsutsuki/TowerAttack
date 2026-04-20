using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.FightMenues;
using GameFields.InputSettings;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public class LoseActions: IActivatable
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

        public void Activate()
        {
            if (_isActive)
                return;

            _isActive = true;

            Activating().ToUniTask();
        }

        private IEnumerator Activating()
        {
            _inputRoot.Deactivate();
            _fightMenu.Deactivate();
            _fightButtonsActivator.Deactivate();
            _hand.ForciblyBlock();
            _boomedTower.Boom();
            _soundController.Stop();

            yield return new WaitForSeconds(5f);

            _bus.Fire(new PersonWinSignal(_loser));
        }
    }
}