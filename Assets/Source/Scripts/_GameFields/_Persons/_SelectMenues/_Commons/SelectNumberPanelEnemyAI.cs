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
                ISelectNumber attackedNumber = GetAttackedNumber() ?? throw new Exception("Ошибка нахождения номера для имитации атаки");
                SelectedNumbers.Add(attackedNumber.Number, NumberAnimationType.Error);
                selectedNumbers.Add(attackedNumber);
            }

            string labelText = "";

            for (int i = 0; i < selectedNumbers.Count; i++)
            {
                if (i != 0)
                    labelText += ",";

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

        private ISelectNumber GetAttackedNumber()
        {
            if (ConfirmableNumbers.Count == _selectNumbers.Length)
            {
                throw new Exception("Не осталось непроверенных номеров!");
            }

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
            foreach (int number in shuffleNumbers)
            {
                ISelectNumber selectNumber = _selectNumbers[number - 1];

                if (ConfirmableNumbers.Contains(selectNumber.Number) == false)
                {
                    return selectNumber;
                }
            }
            return null;
        }
    }
}