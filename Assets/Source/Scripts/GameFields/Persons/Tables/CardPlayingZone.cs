using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Discarding;
using GameFields.Effects;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Tables
{
    public class CardPlayingZone : MonoBehaviour, ICardDropPlace
    {
        private readonly List<Card> _playedCards = new List<Card>();
        
        private Table _table;
        private SignalBus _bus;
        private AppliedEffect _appliedEffect;

        public void Init(Table table, SignalBus bus, EffectFactory effectFactory)
        {
            _table = table;
            _bus = bus;
            _appliedEffect = new AppliedEffect(effectFactory);
        }

        public Vector3 GetPosition() => transform.position;

        public bool TrySeatCard(Card card)
        {
            if (_table.HasFreeSeat == false)
                return false;
            
            card.Play();
            _appliedEffect.Add(card.EffectType);
            _table.SeatCharacter(card.Character);
            _playedCards.Add(card);

            return true;
        }

        public IReadOnlyList<Card> UpdateCards()
        {
            List<Card> toDiscard = new List<Card>();
            
            foreach (Card playedCard in _playedCards)
            {
                playedCard.DecreaseCounter();

                if (playedCard.EffectCounter <= 0)
                {
                    toDiscard.Add(playedCard);
                    _appliedEffect.Remove(playedCard.EffectType);
                    _bus.Fire(new RemoveEffectSignal(playedCard.EffectType));
                }
            }
            
            toDiscard = toDiscard.OrderBy(card => card.Character.transform.position.x).ToList();

            foreach (Card card in toDiscard)
                _playedCards.Remove(card);

            _table.FreeSeats(toDiscard.Select(card => card.Character));
            
            return toDiscard;
        }
    }

    public class AppliedEffect
    {
        private readonly EffectFactory _factory;
        
        private readonly Dictionary<EffectType, int> _effectsCounter;
        private readonly Dictionary<EffectType, Effect> _effects;

        public AppliedEffect(EffectFactory factory)
        {
            _factory = factory;
            _effectsCounter = new Dictionary<EffectType, int>();
            _effects = new Dictionary<EffectType, Effect>();
        }

        //TODO: рассказать мысль
        public void Add(EffectType type)
        {
            Effect effect = _factory.Create(type);
            
            if (_effectsCounter.TryAdd(type, 0))
            {
                _effects.Add(type, effect);
            }
        }

        public void Remove(EffectType type)
        {
            if (!_effectsCounter.ContainsKey(type))
                return;
            
            _effectsCounter[type]--;

            if (_effectsCounter[type] > 0)
                return;
            
            _effectsCounter.Remove(type);
            //_effects[type].Stop();
        }
    }
}
