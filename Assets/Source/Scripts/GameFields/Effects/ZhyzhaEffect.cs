using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private Person _deactivePerson;

        public ZhyzhaEffect(Person deactivePerson) : base(EffectTarget.Self)
        {
            _deactivePerson = deactivePerson;

            PlayEffect();

            CountTurns = 3;
        }

        private void PlayEffect()
        {
//            Debug.Log("Эффект Жыжи");
        }
    }
}
