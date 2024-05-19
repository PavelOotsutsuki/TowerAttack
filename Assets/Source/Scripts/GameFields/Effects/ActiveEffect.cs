using GameFields.Persons;

namespace GameFields.Effects
{
    public class ActiveEffect
    {
        private readonly Person _person;
        private readonly Effect _effect;

        public ActiveEffect(Person person, Effect effect)
        {
            _person = person;
            _effect = effect;
        }

        public int CountTurns => _effect.CountTurns;

        public void DecreaseEffectCounter(Person activePerson)
        {
            if (activePerson == _person)
            {
                _effect.DecreaseCounter();
            }
        }
    }
}
