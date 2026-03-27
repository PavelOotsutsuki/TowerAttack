using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

namespace GameFields.Persons
{
    public class ActiveEffectsList
    {
        private readonly List<PersonEffect> _activeEffects;

        public ActiveEffectsList()
        {
            _activeEffects = new List<PersonEffect>();
        }

        public void Add(PersonEffect personEffect)
        {
            _activeEffects.Add(personEffect);
        }

        public void OnEndTurn()
        {
            foreach (PersonEffect personEffect in _activeEffects)
            {
                personEffect.DecreaseCounter();
            }
        }

        public void Discard(Card card)
        {
            PersonEffect personEffect = _activeEffects.FirstOrDefault(pf => pf.Card == card);

            if (personEffect == null)
                throw new System.Exception("Попытка убрать карту которой нет");

            _activeEffects.Remove(personEffect);
            personEffect.Discard();
        }

        public IReadOnlyList<PersonEffect> TryDiscard()
        {
            if (_activeEffects.Count <= 0)
                return null;

            List<PersonEffect> personEffectsCopy = new List<PersonEffect>();

            foreach (PersonEffect personEffect in _activeEffects)
            {
                if (personEffect.TryDiscard())
                {
                    personEffectsCopy.Add(personEffect);
                }
            }

            if (personEffectsCopy.Count == 0)
                return null;

            foreach (PersonEffect personEffect in personEffectsCopy)
            {
                _activeEffects.Remove(personEffect);
            }

            return personEffectsCopy;
        } 
    }
}
