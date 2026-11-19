using GameFields.Persons;

namespace GameFields.Effects
{
    public class StrongOgre_WeakBlowEffect : StrongOgre_VariantEffect
    {
        private const int CountDrawCards = 1;
        private const int CountAttack = 1;

        public StrongOgre_WeakBlowEffect(Person deactivePerson, EffectData data) :
            base(deactivePerson, CountDrawCards, CountAttack, data)
        { }

        protected override string GetName() => nameof(StrongOgre_WeakBlowEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("End Слабый удар effect");
        //}
    }
}