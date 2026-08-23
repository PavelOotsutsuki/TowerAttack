using System.Collections.Generic;
using Cards;
using Servers;

namespace GameFields.Persons.EffectHandlers.Slimes
{
    public class SlimeEffectHandler : EffectHandler, ITurnSkipper, ILengthyEffectHandler
    {
        private readonly ISlimeEffectWorker _slimeEffectWorker;
        //private readonly List<Card> _turnCardsFromDeck;
        private readonly IDrawnCardClearable _turnDrawnCards;

        private readonly List<Card> _effectedCards;

        public SlimeEffectHandler(ISlimeEffectWorker slimeEffectWorker, IDrawnCardClearable turnDrawnCards,
            FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            _slimeEffectWorker = slimeEffectWorker;
            //_turnCardsFromDeck = new List<Card>();
            _turnDrawnCards = turnDrawnCards;
            _effectedCards = new List<Card>();
        }

        public bool CanSkip => _turnDrawnCards.IsVoid() && _effectedCards.Count > 0;
        //public IEnumerable<Card> TurnCardsFromDeck => _turnCardsFromDeck;

        public void Activate(Card card)
        {
            if (_effectedCards.Contains(card))
                return;

            _effectedCards.Add(card);
            _slimeEffectWorker.Activate();

            if (_effectedCards.Count == 1)
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, null, "ACTIVATE", GetType().Name);

            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "ADD", GetType().Name);
        }

        public void EndEffect(Card card)
        {
            if (_effectedCards.Contains(card) == false)
                return;

            _effectedCards.Remove(card);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "REMOVE", GetType().Name);

            if (_effectedCards.Count == 0)
            {
                _slimeEffectWorker.Deactivate();
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, null, "DEACTIVATE", GetType().Name);
            }
        }

        //void ITurnDrawCardWatcher.SetCard(Card card)
        //{
        //    _turnCardsFromDeck.Add(card);
        //}

        public void OnStartTurn()
        {
            //if (_countTurns > 0)
            //{
            //    _countTurns--;

            //    if (_countTurns == 0)
            //    {
            //        _slimeEffectWorker.Deactivate();
            //    }
            //}

            _turnDrawnCards.Clear();
        }
    }
}