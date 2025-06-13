using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Slimes;
using GameFields.Persons.EffectHandlers.Curses;
using GameFields.Persons.EffectHandlers.Fires;

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

        public PersonEffectsHandler(GnomeEffectHandler gnomeEffectHandler, SlimeEffectHandler slimeEffectHandler,
            CurseEffectHandler curseEffectHandler, FireEffectHandler fireEffectHandler, DoubleEffectHandler doubleEffectHandler,
            SkipTurnEffectHandler skipTurnEffectHandler)
        {
            _gnomeEffectHandler = gnomeEffectHandler;
            _slimeEffectHandler = slimeEffectHandler;
            _curseEffectHandler = curseEffectHandler;
            _fireEffectHandler = fireEffectHandler;
            _doubleEffectHandler = doubleEffectHandler;
            _skipTurnEffectHandler = skipTurnEffectHandler;
        }

        public GnomeEffectHandler GnomeEffectCounter => _gnomeEffectHandler;
        public SlimeEffectHandler SlimeEffectHandler => _slimeEffectHandler;
        public CurseEffectHandler CurseEffectHandler => _curseEffectHandler;
        public FireEffectHandler FireEffectHandler => _fireEffectHandler;
        public DoubleEffectHandler DoubleEffectHandler => _doubleEffectHandler;
        public SkipTurnEffectHandler SkipTurnEffectHandler => _skipTurnEffectHandler;

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
    }
}