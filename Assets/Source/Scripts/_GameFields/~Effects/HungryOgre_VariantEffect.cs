using GameFields.Persons;

namespace GameFields.Effects
{
    public abstract class HungryOgre_VariantEffect : OgreVariantEffect
    {
        public HungryOgre_VariantEffect(Person deactivePerson, int countDrawCards,
            int countAttack, EffectData data) : base(data.ActivePerson, deactivePerson, countDrawCards,
                countAttack, true, data)
        { }
    }
}