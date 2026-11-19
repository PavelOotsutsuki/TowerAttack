using Cards.Effects;
using UnityEngine;

namespace GameFields.Persons
{
    public class PersonEffectKeeper : IReadOnlyPersonEffectKeeper
    {
        private IReadOnlyPersonEffect _lastPersonEffect;

        public PersonEffectKeeper()
        {
            _lastPersonEffect = null;
        }

        public IReadOnlyPersonEffect PersonEffect => _lastPersonEffect;
        public CardEffectConfig LastEffect => _lastPersonEffect == null ? ScriptableObject.CreateInstance<CardEffectConfig>() : _lastPersonEffect.CardEffectConfig;

        public void SetLastPersonEffect(PersonEffect personEffect)
        {
            _lastPersonEffect = personEffect;
        }
    }
}