using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    [CreateAssetMenu(fileName = "New ZhyzhaEffect", menuName = "SO/Create effect/ZhyzhaEffect", order = 51)]
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
