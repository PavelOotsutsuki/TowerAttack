using GameFields.Persons;

namespace GameFields.Effects
{
    public class HungryOgre_HighProfileCrimeEffect : HungryOgre_VariantEffect
    {
        private const int CountDrawCards = 4;
        private const int CountAttack = 4;

        public HungryOgre_HighProfileCrimeEffect(Person deactivePerson, EffectData data) :
            base(deactivePerson, CountDrawCards, CountAttack, data)
        { }

        protected override string GetName() => nameof(HungryOgre_HighProfileCrimeEffect);


        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("End Громкое преступление effect");
        //}
    }
}