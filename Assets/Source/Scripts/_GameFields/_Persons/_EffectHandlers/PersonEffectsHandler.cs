using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers.Slimes;

namespace GameFields.Persons.EffectHandlers
{
    public class PersonEffectsHandler
    {
        private readonly GnomeEffectHandler _gnomeEffectHandler;
        private readonly SlimeEffectHandler _slimeEffectHandler;

        public PersonEffectsHandler(GnomeEffectHandler gnomeEffectHandler, SlimeEffectHandler slimeEffectHandler)
        {
            _gnomeEffectHandler = gnomeEffectHandler;
            _slimeEffectHandler = slimeEffectHandler;
        }

        public GnomeEffectHandler GnomeEffectCounter => _gnomeEffectHandler;
        public SlimeEffectHandler SlimeEffectHandler => _slimeEffectHandler;

        public void OnEndTurn()
        {
            _slimeEffectHandler.OnEndTurn();
        }
    }
}
