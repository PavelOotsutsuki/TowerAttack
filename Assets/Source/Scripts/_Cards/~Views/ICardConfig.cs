using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;

namespace Cards.Views
{
    internal interface ICardConfig
    {
        public ICardViewConfig CardViewConfig { get; }
        public CardCharacter CardCharacter { get; }
        public CardSoundConfig SoundConfig { get; }
        public CardEffectConfig Effect { get; }
        public CardCapability CardCapability { get; }
        public CardPersonType CardPersonType { get; }
        public CardName CardName { get; }
    }
}