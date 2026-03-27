using Cards;
using GameFields.Persons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class GreenGnomeEffect : GnomeEffect
    {
        public GreenGnomeEffect(Person activePerson, EffectData data) : base(activePerson, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Зеленого Гнома закончен");
        }
    }
}