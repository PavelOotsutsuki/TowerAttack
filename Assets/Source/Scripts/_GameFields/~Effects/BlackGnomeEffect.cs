using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class BlackGnomeEffect : GnomeEffect
    {
        public BlackGnomeEffect(Person activePerson, EffectData data) : base(activePerson, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Черного Гнома закончен");
        }
    }
}