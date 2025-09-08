using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class RedGnomeEffect : GnomeEffect
    {
        public RedGnomeEffect(Person activePerson, SignalBus bus, CardEffectData data) : base(activePerson, bus, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Красного Гнома закончен");
        }
    }
}