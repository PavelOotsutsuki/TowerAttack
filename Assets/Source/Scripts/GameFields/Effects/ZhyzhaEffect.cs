using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private Person _deactivePerson;

        public ZhyzhaEffect(Card card, Person deactivePerson) 
            : base(card, EffectTarget.Self, 3)
        {
            _deactivePerson = deactivePerson;

            PlayEffect();
        }

        private void PlayEffect()
        {
//            Debug.Log("Эффект Жыжи");
        }
    }
}
