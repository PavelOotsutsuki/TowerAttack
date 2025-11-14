using UnityEngine;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class HungryOgre_SilentSearchEffect : HungryOgre_VariantEffect
    {
        private const int CountDrawCards = 1;
        private const int CountAttack = 1;

        public HungryOgre_SilentSearchEffect(Person activePerson, Person deactivePerson, EffectData data) :
            base(activePerson, deactivePerson, CountDrawCards, CountAttack, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("End Тихий поиск effect");
        }
    }
}