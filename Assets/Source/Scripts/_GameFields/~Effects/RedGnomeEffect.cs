using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class RedGnomeEffect : GnomeEffect
    {
        public RedGnomeEffect(Person activePerson, EffectData data) : base(activePerson, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Красного Гнома закончен");
        }
    }
}