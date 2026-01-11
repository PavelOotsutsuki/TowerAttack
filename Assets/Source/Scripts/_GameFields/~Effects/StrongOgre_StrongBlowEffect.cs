using UnityEngine;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class StrongOgre_StrongBlowEffect : StrongOgre_VariantEffect
    {
        private const int CountDrawCards = 4;
        private const int CountAttack = 4;

        public StrongOgre_StrongBlowEffect(Person deactivePerson, EffectData data) :
            base(deactivePerson, CountDrawCards, CountAttack, data)
        { }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("End Сильный удар effect");
        //}
    }
}