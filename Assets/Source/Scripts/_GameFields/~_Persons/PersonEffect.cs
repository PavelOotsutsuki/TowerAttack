using Cards;
using Cards.Effects;
using GameFields.Effects;
using Servers;

namespace GameFields.Persons
{
    public class PersonEffect: IReadOnlyPersonEffect
    {
        private readonly CardEffectConfigPair _cardEffectConfigPair;
        private readonly Effect _effect;
        private readonly EffectDuration _effectDuration;
        private readonly FightProcessDBManager _fightProcessDBManager;
        private readonly bool _isPlayersEffect;

        public PersonEffect(Effect effect, EffectDuration effectDuration, CardEffectConfigPair cardEffectConfigPair,
            FightProcessDBManager fightProcessDBManager, bool isPlayersEffect)
        {
            _cardEffectConfigPair = cardEffectConfigPair;
            _effect = effect;
            _fightProcessDBManager = fightProcessDBManager;
            _isPlayersEffect = isPlayersEffect;

            _effectDuration = effectDuration;
        }

        public Card Card => _cardEffectConfigPair.Card;
        public CardEffectConfig CardEffectConfig => _cardEffectConfigPair.CardEffectConfig;
        public Effect Effect => _effect;

        public void Discard()
        {
            _effectDuration.Discard();
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersEffect, Card.ViewData.Number.ToString(), "Forcibly Discard", GetType().Name);

            TryDiscard();
        }

        public void DecreaseCounter()
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersEffect, Card.ViewData.Number.ToString(), $"LEFT ({_effectDuration.Duration}) TURNS", GetType().Name);
            _effectDuration.Decrease();
        }

        public bool TryDiscard()
        {
            if (_effectDuration.CanDiscard)
            {
                _effect?.End();
                return true;
            }

            DecreaseCounter();
            return false;
        }
    }
}