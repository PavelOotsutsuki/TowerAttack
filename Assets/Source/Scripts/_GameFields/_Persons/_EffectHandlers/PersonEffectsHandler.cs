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

        public PersonEffectsHandler(GnomeEffectHandler gnomeEffectHandler, SlimeEffectHandler slimeEffectHandler,
            CurseEffectHandler curseEffectHandler, FireEffectHandler fireEffectHandler)
        {
            _gnomeEffectHandler = gnomeEffectHandler;
            _slimeEffectHandler = slimeEffectHandler;
            _curseEffectHandler = curseEffectHandler;
            _fireEffectHandler = fireEffectHandler;
        }

        public GnomeEffectHandler GnomeEffectCounter => _gnomeEffectHandler;
        public SlimeEffectHandler SlimeEffectHandler => _slimeEffectHandler;
        public CurseEffectHandler CurseEffectHandler => _curseEffectHandler;
        public FireEffectHandler FireEffectHandler => _fireEffectHandler;

        public void OnStartTurn()
        {
            _curseEffectHandler.OnStartTurn();
        }

        public void OnEndTurn()
        {
            _slimeEffectHandler.OnEndTurn();
            _fireEffectHandler.OnEndTurn();
        }
    }
}
