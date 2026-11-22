using System;
using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Tools;

namespace Cards.Views
{
    internal interface ICardConfig : IData
    {
        public ICardViewConfig CardViewConfig { get; }
        // public CardCharacter CardCharacter { get; }
        public ICardSoundConfig SoundConfig { get; }
        public ICardEffectConfig Effect { get; }
        public CardCapability CardCapability { get; }
        public CardPersonType CardPersonType { get; }
        public CardName CardName { get; }
        public Guid CardGlobalGuid { get; }
    }
}