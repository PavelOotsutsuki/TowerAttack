using GameFields.Persons;

namespace GameFields.Effects
{
    public abstract class StrongOgre_VariantEffect : OgreVariantEffect
    {
        public StrongOgre_VariantEffect(Person deactivePerson, int countDrawCards,
            int countAttack, EffectData data) : base(deactivePerson, data.ActivePerson, countDrawCards,
                countAttack, false, data)
        { }
    }
}