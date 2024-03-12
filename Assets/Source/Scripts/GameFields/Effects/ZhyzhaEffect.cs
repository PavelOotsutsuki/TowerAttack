using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect
    {
        private IPerson _deactivePerson;

        public ZhyzhaEffect(IPerson deactivePerson)
        {
            _deactivePerson = deactivePerson;

            PlayEffect();
        }

        private void PlayEffect()
        {
            Debug.Log("Эффект Жыжи");
        }
    }
}
