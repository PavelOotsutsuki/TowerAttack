using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class BlueGnomeEffect : GnomeEffect
    {
        public BlueGnomeEffect(Person activePerson, EffectData data) : base(activePerson, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Синего Гнома закончен");
        }
    }
}