using GameFields.Persons.Commons;
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