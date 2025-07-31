using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Slimes;
using GameFields.Persons.EffectHandlers.Curses;
using GameFields.Persons.EffectHandlers.Fires;
using System;
using Tools;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons.EffectHandlers
{
    public class PersonEffectsHandler
    {
        private readonly GnomeEffectHandler _gnomeEffectHandler;
        private readonly SlimeEffectHandler _slimeEffectHandler;
        private readonly CurseEffectHandler _curseEffectHandler;
        private readonly FireEffectHandler _fireEffectHandler;
        private readonly DoubleEffectHandler _doubleEffectHandler;
        private readonly SkipTurnEffectHandler _skipTurnEffectHandler;
        private readonly FateInevitabilityHandler _fateInevitabilityHandler;

        public PersonEffectsHandler(GnomeEffectHandler gnomeEffectHandler, SlimeEffectHandler slimeEffectHandler,
            CurseEffectHandler curseEffectHandler, FireEffectHandler fireEffectHandler, DoubleEffectHandler doubleEffectHandler,
            SkipTurnEffectHandler skipTurnEffectHandler, FateInevitabilityHandler fateInevitabilityHandler)
        {
            _gnomeEffectHandler = gnomeEffectHandler;
            _slimeEffectHandler = slimeEffectHandler;
            _curseEffectHandler = curseEffectHandler;
            _fireEffectHandler = fireEffectHandler;
            _doubleEffectHandler = doubleEffectHandler;
            _skipTurnEffectHandler = skipTurnEffectHandler;
            _fateInevitabilityHandler = fateInevitabilityHandler;
        }

        public GnomeEffectHandler GnomeEffectCounter => _gnomeEffectHandler;
        public SlimeEffectHandler SlimeEffectHandler => _slimeEffectHandler;
        public CurseEffectHandler CurseEffectHandler => _curseEffectHandler;
        public FireEffectHandler FireEffectHandler => _fireEffectHandler;
        public DoubleEffectHandler DoubleEffectHandler => _doubleEffectHandler;
        public SkipTurnEffectHandler SkipTurnEffectHandler => _skipTurnEffectHandler;
        public FateInevitabilityHandler FateInevitabilityHandler => _fateInevitabilityHandler;

        public void OnStartTurn()
        {
            _curseEffectHandler.OnStartTurn();
        }

        public void OnEndTurn()
        {
            _doubleEffectHandler.OnEndTurn();
            _skipTurnEffectHandler.OnEndTurn();
            _slimeEffectHandler.OnEndTurn();
            _fireEffectHandler.OnEndTurn();
        }

        public void BeforeEndTurn(Action callback)
        {
            //callback.Invoke();
            //return;

            CallbackHandler fateInevitabilityHandlerCallbackHandler = new CallbackHandler();
            _fateInevitabilityHandler.BeforeEndTurn(fateInevitabilityHandlerCallbackHandler);

            List<ICompletable> completables = new List<ICompletable>();

            completables.Add(fateInevitabilityHandlerCallbackHandler);

            WaitingAllBeforeEndTurnActions(completables, callback).ToUniTask();
        }

        private IEnumerator WaitingAllBeforeEndTurnActions(IEnumerable<ICompletable> completables, Action callback)
        {
            yield return new WaitUntil(() => completables.Any(c => c.IsComplete == false) == false);

            callback?.Invoke();
        }

        //private IEnumerator WaitingAllBeforeEndTurnActions(ICompletable completables, Action callback)
        //{
        //    yield return new WaitUntil(() => completables.IsComplete);

        //    callback?.Invoke();
        //}
    }
}