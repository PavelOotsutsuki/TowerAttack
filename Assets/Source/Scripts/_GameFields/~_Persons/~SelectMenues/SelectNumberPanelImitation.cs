using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using Tools.UI;
using UnityEngine;
using Zenject;
using System.Linq;
using Random = UnityEngine.Random;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.Towers;
using GameFields.Persons;
using GameFields.Persons.ConfirmableNumbersView;

namespace GameFields.Persons.SelectMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class SelectNumberPanelImitation : SelectNumberPanel
    {
        [SerializeField] private SelectNumberPanelImitatitonData _data;

        private SelectNumberImitation[] _selectNumbers;

        //private InformationLabel _informationLabel;

        //[Inject]
        //public void Construct(InformationLabel informationLabel)
        //{
        //    _informationLabel = informationLabel;
        //}

        public void Init(ICardNumberKeeper cardNumberKeeper, int[] сardNumbers, SelectNumbersList selectedNumbers,
            ConfirmableNumbers confirmableNumbers, LastSelectedNumbersWatcher lastSelectedNumbersWatcher)
        {
            _selectNumbers = new SelectNumberImitation[сardNumbers.Length];

            base.Init(cardNumberKeeper, сardNumbers, selectedNumbers, confirmableNumbers, _selectNumbers,
                lastSelectedNumbersWatcher);
        }

        protected override void InitNumbers()
        {
            for (int i = 0; i < CardNumbers.Length; i++)
            {
                SelectNumberImitation selectNumber = new SelectNumberImitation(i + 1);
                _selectNumbers[i] = selectNumber;
            }
        }

        protected override void OnActivate()
        {
            Selecting().ToUniTask();
        }

        protected override IEnumerator Deactivating()
        {
            FadablePanel.Hide();

            //yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => FadablePanel.IsComplete);

            IsCompleteThis = true;
        }

        private IEnumerator Selecting()
        {
            //yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => FadablePanel.IsComplete);
            yield return new WaitForSeconds(_data.DelayThinkImitation / 2f);
            GameFieldGC.Collect();
            yield return new WaitForSeconds(_data.DelayThinkImitation / 2f);

            IRandomSelectNumberLogic selectNumberLogic = IsConsecutiveMode ?
                new ConsecutiveRandomSelectNumberLogic(NeedForActivate, CurrentAvailableNumbers, ConfirmableNumbers) :
                new DefaultRandomSelectNumberLogic(NeedForActivate, CurrentAvailableNumbers, ConfirmableNumbers);

            IReadOnlyList<ISelectNumber> selectedNumbers = selectNumberLogic.GetSelectedNumbers();
            //List<ISelectNumber> restrictionNumbers = new List<ISelectNumber>();

            //foreach (ISelectNumber selectNumber in _selectNumbers)
            //{
            //    if (CurrentAvailableNumbers.Contains(selectNumber) == false)
            //        restrictionNumbers.Add(selectNumber);
            //}

            List<int> lastSelectedNumbers = new List<int>(); 

            string labelText = "";

            for (int i = 0; i < selectedNumbers.Count; i++)
            {
                if (i != 0)
                    labelText += ", ";

                labelText += selectedNumbers[i].Number.ToString();
                lastSelectedNumbers.Add(selectedNumbers[i].Number);
            }

            LastSelectedNumbersWatcher.SetNumbers(lastSelectedNumbers);

            //LabelActivateData informationLableData = new LabelActivateData(labelText);
            //_informationLabel.Activate(informationLableData);

            //yield return new WaitForSeconds(_data.TimeViewInformationLabel);
            //_informationLabel.Deactivate();

            //yield return new WaitUntil(() => _informationLabel.IsComplete);

            ResultType resultType = ResultType.Falled;

            foreach (ISelectNumber selectedNumber in selectedNumbers)
            {
                if (CardNumberKeeper.Card.IsSuccessChoice(selectedNumber.Number))
                {
                    resultType = ResultType.Success;
                    break;
                }
            }

            if (resultType == ResultType.Falled)
            {
                foreach (ISelectNumber selectedNumber in selectedNumbers)
                {
                    SelectedNumbers.Add(selectedNumber.Number, NumberAnimationType.Choice);
                }
            }
            else if (_data.IsRememberSuccessChoice)
            {
                foreach (ISelectNumber selectNumber in _selectNumbers)
                {
                    if (selectedNumbers.Contains(selectNumber) == false)
                    {
                        SelectedNumbers.Add(selectNumber.Number, NumberAnimationType.Choice);
                    }
                }
            }

            SetSelectResultData setSelectResultData = new SetSelectResultData(resultType, labelText);
            SelectResult.SetResult(setSelectResultData);

            #region DEBUG_ENEMY_NUMBERS
            string debugMsg = "";

            foreach (KeyValuePair<int, NumberAnimationType> selectNumber in ConfirmableNumbers.FullList.SelectedNumbersStates.OrderByDescending(n => n.Key))
            {
                if (debugMsg != "")
                    debugMsg += ",";

                debugMsg += selectNumber.Key.ToString();
            }

            Debug.Log(debugMsg);
            #endregion

            IsCompleteThis = true;
        }
    }
}