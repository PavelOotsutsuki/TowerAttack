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

namespace GameFields.Persons.SelectMenues.Commons
{
    [RequireComponent(typeof(FadablePanel))]
    public class SelectNumberPanelEnemyAI : SelectNumberPanel
    {
        [SerializeField] private SelectNumberPanelEnemyAIData _data;

        private SelectNumberImitation[] _selectNumbers;

        //private InformationLabel _informationLabel;

        //[Inject]
        //public void Construct(InformationLabel informationLabel)
        //{
        //    _informationLabel = informationLabel;
        //}

        protected override void InitNumbers()
        {
            _selectNumbers = new SelectNumberImitation[CountNumbers];

            for (int i = 0; i < CountNumbers; i++)
            {
                SelectNumberImitation attackNumber = new SelectNumberImitation(i + 1);
                _selectNumbers[i] = attackNumber;
            }
        }

        protected override void OnActivate()
        {
            Attacking().ToUniTask();
        }

        protected override IEnumerator Deactivating()
        {
            FadablePanel.Hide();

            //yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => FadablePanel.IsComplete);

            IsCompleteThis = true;
        }

        private IEnumerator Attacking()
        {
            //yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => FadablePanel.IsComplete);
            yield return new WaitForSeconds(_data.DelayThinkImitation);

            List<ISelectNumber> selectedNumbers = new List<ISelectNumber>();

            for (int i = 0; i < NeedForActivate; i++)
            {
                ISelectNumber selectedNumber = GetSelectedNumber(selectedNumbers) ?? throw new Exception("Ошибка нахождения номера для имитации атаки");
                selectedNumbers.Add(selectedNumber);
            }

            string labelText = "";

            for (int i = 0; i < selectedNumbers.Count; i++)
            {
                if (i != 0)
                    labelText += ", ";

                labelText += selectedNumbers[i].Number.ToString();
            }

            //LabelActivateData informationLableData = new LabelActivateData(labelText);
            //_informationLabel.Activate(informationLableData);

            //yield return new WaitForSeconds(_data.TimeViewInformationLabel);
            //_informationLabel.Deactivate();

            //yield return new WaitUntil(() => _informationLabel.IsComplete);

            ResultType resultType = ResultType.Falled;

            foreach (ISelectNumber selectedNumber in selectedNumbers)
            {
                if (CardNumberKeeper.Card.IsSuccessAttack(selectedNumber.Number))
                {
                    resultType = ResultType.Success;
                    break;
                }
            }

            if (resultType == ResultType.Falled)
            {
                foreach (ISelectNumber selectedNumber in selectedNumbers)
                {
                    SelectedNumbers.Add(selectedNumber.Number, NumberAnimationType.Error);
                }
            }
            else
            {
                foreach (ISelectNumber selectNumber in _selectNumbers)
                {
                    if (selectedNumbers.Contains(selectNumber) == false)
                    {
                        SelectedNumbers.Add(selectNumber.Number, NumberAnimationType.Error);
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

        private ISelectNumber GetSelectedNumber(List<ISelectNumber> exceptionsNumbers)
        {
            List<int> shuffleNumbers = new List<int>();
            List<int> allNumbers = new List<int>();

            for (int i = 0; i < CountNumbers; i++)
            {
                allNumbers.Add(i + 1);
            }

            while (allNumbers.Count > 0)
            {
                int selectNumber = allNumbers[Random.Range(0, allNumbers.Count)];
                shuffleNumbers.Add(selectNumber);
                allNumbers.Remove(selectNumber);
            }

            if (ConfirmableNumbers.Count + exceptionsNumbers.Count >= _selectNumbers.Length) // Если все возможные варианты разработаны, берем уже проверенные номера
            {
                if (ConfirmableNumbers.Count + exceptionsNumbers.Count == _selectNumbers.Length)
                {
                    Debug.Log("Номера кончились");
                }
                else
                {
                    string numbers = "";

                    foreach (ISelectNumber number in exceptionsNumbers)
                    {
                        if (numbers != "")
                            numbers += ",";

                        numbers += number.Number.ToString();
                    }

                    Debug.Log("Номеров меньше чем сумма номеров: " + numbers);
                }

                foreach (int number in shuffleNumbers)
                {
                    ISelectNumber selectNumber = _selectNumbers[number - 1];

                    if (exceptionsNumbers.Contains(selectNumber) == false)
                    {
                        return selectNumber;
                    }
                }
            }

            foreach (int number in shuffleNumbers)
            {
                ISelectNumber selectNumber = _selectNumbers[number - 1];

                if (ConfirmableNumbers.Contains(selectNumber.Number) == false && exceptionsNumbers.Contains(selectNumber) == false)
                {
                    return selectNumber;
                }
            }

            return null;
        }
    }
}