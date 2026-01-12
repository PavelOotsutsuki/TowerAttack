using Cards;
using GameFields.Persons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class WhiteGnomeEffect : GnomeEffect
    {
        public WhiteGnomeEffect(Person activePerson, EffectData data) : base(activePerson, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Белого Гнома закончен");
        }
    }
}