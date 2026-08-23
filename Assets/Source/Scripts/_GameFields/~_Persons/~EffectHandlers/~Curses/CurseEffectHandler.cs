using System.Collections.Generic;
using Cards;
using GameFields.InformationLabels;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.Towers;
using Servers;
using Tools.UI;
using Tools.Utils;

namespace GameFields.Persons.EffectHandlers.Curses
{
    public abstract class CurseEffectHandler: EffectHandler, ILengthyEffectHandler
    {
        private readonly ICardNumberKeeper _tower;
        private readonly InformationLabel _informationLabel;
        private readonly ConfirmableNumbers _confirmableNumbers;
        private readonly SelectNumbersList _cursedList;

        //private readonly List<ICompletable> _cursedEffectCards;
        private readonly List<Card> _effectedCards;
        private readonly List<CurseEffectWithoutCard> _effectsWithoutCards;

        private bool _deactivateMode = false;

        public CurseEffectHandler(ICardNumberKeeper tower, InformationLabel informationLabel, ConfirmableNumbers confirmableNumbers,
            SelectNumbersList cursedList, FightProcessDBManager fightProcessDBManager, bool isPlayersObject) :
            base(fightProcessDBManager, isPlayersObject)
        {
            _tower = tower;
            _informationLabel = informationLabel;

            _confirmableNumbers = confirmableNumbers;
            _cursedList = cursedList;
            //_effectedCards = new List<ICompletable>();
            _effectedCards = new List<Card>();
            _effectsWithoutCards = new List<CurseEffectWithoutCard>();
            _deactivateMode = false;
        }

        //public void Add(ICompletable effectedCard)
        //{
        //    _effectedCards.Add(effectedCard);
        //}

        public void Activate(Card card)
        {
            //if (_effectedCards.Contains(card))
            //    return;

            _effectedCards.Add(card);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "ADD BY CARD", GetType().Name);
        }

        public void Activate(int countTurns)
        {
            _effectsWithoutCards.Add(new CurseEffectWithoutCard(countTurns));
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, countTurns.ToString(), "ADD ON TURNS", GetType().Name);
        }

        public void SetDeactivateMode(bool isDeactivateMode)
        {
            _deactivateMode = isDeactivateMode;
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _deactivateMode ? "ACTIVATE" : "DEACTIVATE", "CURSE IMMUNITY", GetType().Name);
        }

        public void EndEffect(Card card)
        {
            while (_effectedCards.Contains(card))
            {
                _effectedCards.Remove(card);
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "REMOVE BY CARD", GetType().Name);
            }
        }

        public void OnStartTurn()
        {
            //if (_effectedCards.Count == 0)
            //    return;

            //for (int i = _effectedCards.Count - 1; i >= 0; i--)
            //{
            //    if (_effectedCards[i].IsComplete)
            //        _effectedCards.Remove(_effectedCards[i]);
            //}

            //foreach (ICompletable effectCard in _cursedEffectCards)
            //{
            //    if (effectCard.IsComplete)
            //        _cursedEffectCards.Remove(effectCard);
            //}

            int cursedCount = _effectedCards.Count + _effectsWithoutCards.Count;
            //Debug.Log($"cursedCount = {cursedCount}, _deactivateMode = {_deactivateMode}");
            if (cursedCount == 0 || _deactivateMode)
                return;

            string cursedMessage = "";
            IEnumerable<int> shuffleCardNumbers = Utils.Shuffle(_confirmableNumbers.FreeNumbers);
            List<int> findedNumbers = new List<int>();

            for (int i = 0; i < cursedCount; i++)
            {
                bool isFinded = false;

                foreach (int cardNumber in shuffleCardNumbers)
                {
                    if (_confirmableNumbers.Contains(cardNumber) == false && _tower.Card.IsSuccessChoice(cardNumber) == false)
                    {
                        //_cursedList.Add(cardNumber, NumberAnimationType.Curse);
                        _cursedList.Add(cardNumber);

                        findedNumbers.Add(cardNumber);

                        isFinded = true;
                        break;
                    }
                }

                if (isFinded == false)
                {
                    foreach (int cardNumber in shuffleCardNumbers)
                    {
                        if (findedNumbers.Contains(cardNumber) == false && _tower.Card.IsSuccessChoice(cardNumber) == false)
                        {
                            //_cursedList.Add(cardNumber, NumberAnimationType.Curse);
                            _cursedList.Add(cardNumber);

                            findedNumbers.Add(cardNumber);
                            break;
                        }
                    }
                }
            }

            foreach (int number in findedNumbers)
            {
                if (cursedMessage != "")
                    cursedMessage += ", ";

                cursedMessage += number.ToString();
            }

            string personCurseFeature = GetPersonFeature();

            LabelActivateData labelActivateData = new LabelActivateData($"<b>{personCurseFeature}</b>\n{cursedMessage}");
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 2f);
            _informationLabel.Activate(informationLabelActivateData);

            for (int i = 0; i < _effectsWithoutCards.Count; i++ )
            {
                _effectsWithoutCards[i].NextTurn();

                if (_effectsWithoutCards[i].CanBeDestroy)
                {
                    _effectsWithoutCards.Remove(_effectsWithoutCards[i]);
                    i--;
                }
            }

            PushStep();
        }

        protected abstract string GetPersonFeature();
        protected abstract void PushStep();
    }
}