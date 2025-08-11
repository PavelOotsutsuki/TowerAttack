using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class GreenGnomeEffect : GnomeEffect
    {
        public GreenGnomeEffect(Person activePerson) : base(activePerson)
        { }

        public override void End()
        {
            Debug.Log("Эффект Зеленого Гнома закончен");
        }
    }
}