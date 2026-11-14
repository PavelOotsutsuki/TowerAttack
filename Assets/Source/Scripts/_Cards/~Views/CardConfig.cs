using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Views
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Cards/CardConfig", order = 51)]
    internal class CardConfig : ScriptableObject, IData, ICardConfig
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

        public ICardViewConfig CardViewConfig => _cardViewConfig;
        public CardCharacter CardCharacter => _cardCharacter;
        public CardSoundConfig SoundConfig => _soundConfig;
        public CardEffectConfig Effect => _effect;
        public CardCapability CardCapability => _cardCapability;
        public CardPersonType CardPersonType => _cardPersonType;
        public CardName CardName => _cardName;
    }
}