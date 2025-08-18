using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class RedGnomeEffect : GnomeEffect
    {
        public RedGnomeEffect(Person activePerson) : base(activePerson)
        { }

        public override void End()
        {
            Debug.Log("Эффект Красного Гнома закончен");
        }
    }
}