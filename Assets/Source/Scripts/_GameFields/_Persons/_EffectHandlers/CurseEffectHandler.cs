using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.InformationLabels;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils;
using UnityEngine;

namespace GameFields.Persons.Commons
{
    public class CurseEffectHandler
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
        }

        public void OnStartTurn()
        {
            foreach (ICompletable effectCard in _cursedEffectCards)
            {
                if (effectCard.IsComplete)
                    _cursedEffectCards.Remove(effectCard);
            }

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

            LabelActivateData labelActivateData = new LabelActivateData("<b>ПРОКЛЯТЬЕ:</b>\n" + cursedMessage);
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
            _informationLabel.Activate(informationLabelActivateData);
        }
    }
}
