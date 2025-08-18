using GameFields.Persons.Common;
using UnityEngine;

namespace GameFields.Effects
{
    public class BlueGnomeEffect : GnomeEffect
    {
        public BlueGnomeEffect(Person activePerson) : base(activePerson)
        { }

        public override void End()
        {
            Debug.Log("Эффект Синего Гнома закончен");
        }
    }
}