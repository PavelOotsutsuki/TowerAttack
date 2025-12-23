using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Commons
{
    public class LoseActions: IActivatable
    {
        private readonly IBoomTower _boomedTower;
        private readonly IPersonObject _loser;
        private readonly IHandBlockable _hand;
        private readonly SignalBus _bus;

        public LoseActions(IBoomTower boomedTower, IPersonObject loser, IHandBlockable handBlockable, SignalBus bus)
        {
            _boomedTower = boomedTower;
            _loser = loser;
            _hand = handBlockable;
            _bus = bus;
        }

        public void Activate()
        {
            Activating().ToUniTask();
        }

        private IEnumerator Activating()
        {
            _hand.ForciblyBlock();
            _boomedTower.Boom();

            yield return new WaitForSeconds(5f);

            _bus.Fire(new PersonWinSignal(_loser));
        }
    }
}