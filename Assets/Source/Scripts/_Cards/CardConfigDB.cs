using System;
using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Cards.Views;
using UnityEngine;

namespace Cards
{
    public class CardConfigDB : ICardConfig
    {
        private readonly CardSoundConfigDB _soundConfig;
        private readonly ICardEffectConfig _effect;
        private readonly CardCapability _cardCapability;
        private readonly CardPersonType _cardPersonType;
        private readonly CardName _cardName;
        private readonly ICardViewConfig _cardViewConfig;

        private readonly Guid _cardGlobalGuid;

        public CardConfigDB(CardSoundConfigDB cardSoundConfigDB)
        {
            _soundConfig = cardSoundConfigDB;
        }
        public ICardSoundConfig SoundConfig => _soundConfig;

        public ICardEffectConfig Effect => _effect;

        public CardCapability CardCapability => _cardCapability;

        public CardPersonType CardPersonType => _cardPersonType;

        public CardName CardName => _cardName;

        public ICardViewConfig CardViewConfig => _cardViewConfig;

        public Guid CardGlobalGuid => _cardGlobalGuid;

        // public CardCharacter CardCharacter => null;
    }
}
