using GameFields.Persons;

namespace GameFields.Effects
{
    public abstract class StrongOgre_VariantEffect : OgreVariantEffect
    {
        public StrongOgre_VariantEffect(Person activePerson, Person deactivePerson, int countDrawCards,
            int countAttack, EffectData data) : base(deactivePerson, activePerson, countDrawCards,
                countAttack, false, data)
        { }
    }
}