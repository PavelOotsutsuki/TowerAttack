using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect: Effect
    {
        private IPerson _deactivePerson;

        public ZhyzhaEffect(IPerson deactivePerson)
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
