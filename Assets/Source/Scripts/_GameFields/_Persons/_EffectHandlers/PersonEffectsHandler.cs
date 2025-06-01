using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Slimes;
using GameFields.Persons.EffectHandlers.Curses;

namespace GameFields.Persons.EffectHandlers
{
    public class PersonEffectsHandler
    {
        private readonly GnomeEffectHandler _gnomeEffectHandler;
        private readonly SlimeEffectHandler _slimeEffectHandler;
        private readonly CurseEffectHandler _curseEffectHandler;

        public PersonEffectsHandler(GnomeEffectHandler gnomeEffectHandler, SlimeEffectHandler slimeEffectHandler,
            CurseEffectHandler curseEffectHandler)
        {
            _gnomeEffectHandler = gnomeEffectHandler;
            _slimeEffectHandler = slimeEffectHandler;
            _curseEffectHandler = curseEffectHandler;
        }

        public GnomeEffectHandler GnomeEffectCounter => _gnomeEffectHandler;
        public SlimeEffectHandler SlimeEffectHandler => _slimeEffectHandler;
        public CurseEffectHandler CurseEffectHandler => _curseEffectHandler;

        public void OnStartTurn()
        {
            _curseEffectHandler.OnStartTurn();
        }

        public void OnEndTurn()
        {
            _slimeEffectHandler.OnEndTurn();
        }
    }
}
