using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private Person _deactivePerson;

        public ZhyzhaEffect(Person deactivePerson) : base(EffectTarget.Opponent)
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
