using System.Collections.Generic;
using Cards;
using GameFields.InformationLabels;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Curses
{
    public abstract class CurseEffectHandler
    {
        private readonly ICardNumberKeeper _tower;
        private readonly InformationLabel _informationLabel;
        private readonly ConfirmableNumbers _confirmableNumbers;
        private readonly SelectNumbersList _cursedList;

        private readonly List<ICompletable> _cursedEffectCards;

        public CurseEffectHandler(ICardNumberKeeper tower, InformationLabel informationLabel, ConfirmableNumbers confirmableNumbers,
            SelectNumbersList cursedList)
        {
            _tower = tower;
            _informationLabel = informationLabel;

            _confirmableNumbers = confirmableNumbers;
            _cursedList = cursedList;
            _cursedEffectCards = new List<ICompletable>();
        }

        public void Add(ICompletable effectedCard)
        {
            _cursedEffectCards.Add(effectedCard);
            Debug.Log("Add" + " /// " + this);
        }

        public void OnStartTurn()
        {
            if (_cursedEffectCards.Count == 0)
                return;

            for (int i = _cursedEffectCards.Count - 1; i >= 0; i--)
            {
                if (_cursedEffectCards[i].IsComplete)
                    _cursedEffectCards.Remove(_cursedEffectCards[i]);
            }

            //foreach (ICompletable effectCard in _cursedEffectCards)
            //{
            //    if (effectCard.IsComplete)
            //        _cursedEffectCards.Remove(effectCard);
            //}

            int cursedCount = _cursedEffectCards.Count;

            if (cursedCount == 0)
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
                        _cursedList.Add(cardNumber, NumberAnimationType.Curse);

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
                            _cursedList.Add(cardNumber, NumberAnimationType.Curse);

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

            PushStep();
        }

        protected abstract string GetPersonFeature();
        protected abstract void PushStep();
    }
}