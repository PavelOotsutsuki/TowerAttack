using System;
using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Views
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Cards/CardConfig", order = 51)]
    internal class CardConfig : ScriptableObject, ICardConfig
    {
        [FormerlySerializedAs("<CardViewConfig>k__BackingField")]
        [SerializeField] private CardViewConfig _cardViewConfig;
        [FormerlySerializedAs("<CardCharacter>k__BackingField")]
        [SerializeField] private CardCharacter _cardCharacter;
        [FormerlySerializedAs("<SoundConfig>k__BackingField")]
        [SerializeField] private CardSoundConfig _soundConfig;
        [FormerlySerializedAs("<Effect>k__BackingField")]
        [SerializeField] private CardEffectConfig _effect;
        [FormerlySerializedAs("<CardCapability>k__BackingField")]
        [SerializeField] private CardCapability _cardCapability;
        [FormerlySerializedAs("<CardPersonType>k__BackingField")]
        [SerializeField] private CardPersonType _cardPersonType;
        [FormerlySerializedAs("<CardName>k__BackingField")]
        [SerializeField] private CardName _cardName;

        public void SetSoundConfig(CardSoundConfigDB cardSoundConfigDB)
        {
            if (cardSoundConfigDB != null)
            {
                _cardSoundConfigDB = cardSoundConfigDB;
            }
            else
            {
                _cardSoundConfigDB = _soundConfig;
            }
        }

        public void SetViewConfig(CardViewConfigDB cardViewConfigDB)
        {
            if (cardViewConfigDB != null)
            {
                _cardViewConfigDB = cardViewConfigDB;
            }
            else
            {
                _cardViewConfigDB = _cardViewConfig;
            }
        }

        private ICardViewConfig _cardViewConfigDB;
        private ICardSoundConfig _cardSoundConfigDB;

        private Guid _cardGlobalGuid = Guid.NewGuid();

        public ICardViewConfig CardViewConfig => _cardViewConfigDB;
        public CardCharacter CardCharacter => _cardCharacter;
        public ICardSoundConfig SoundConfig => _cardSoundConfigDB;
        // public ICardSoundConfig SoundConfig => _soundConfig;
        public ICardEffectConfig Effect => _effect;
        public CardCapability CardCapability => _cardCapability;
        public CardPersonType CardPersonType => _cardPersonType;
        public CardName CardName => _cardName;

        public Guid CardGlobalGuid => _cardGlobalGuid;
    }
}