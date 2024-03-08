using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : CardEffect
    {
        private IPerson _deactivePerson;

        public void Init(IPerson deactivePerson)
        {
            _deactivePerson = deactivePerson;
        }

        public override void Play()
        {
            Debug.Log("Эффект Жыжи");
        }
    }
}
