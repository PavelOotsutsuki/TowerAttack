using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : IEffect
    {
        private Person _deactivePerson;

        public ZhyzhaEffect(Person deactivePerson) 
        {
            _deactivePerson = deactivePerson;
        }

        public void Play()
        {
            Debug.Log("Эффект Жыжи");
        }

        public void End()
        {
            Debug.Log("Эффект Жыжи закончен");
        }
    }
}
