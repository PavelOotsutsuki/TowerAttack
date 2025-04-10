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

namespace GameFields.Persons.SelectMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class SelectNumberPanelEnemyAI : SelectNumberPanel
    {
        [SerializeField] private SelectNumberPanelEnemyAIData _data;

        private SelectNumberImitation[] _selectNumbers;

        private InformationLableRoot _informationLableRoot;

        [Inject]
        public void Construct(InformationLableRoot informationLableRoot)
        {
            _informationLableRoot = informationLableRoot;
        }

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
                ConfirmableNumbers.AddSelect(attackedNumber);
                selectedNumbers.Add(attackedNumber);
            }

            string labelText = _data.DefaultInformationLabelText;

            for (int i = 0; i < selectedNumbers.Count; i++)
            {
                if (i != 0)
                    labelText += ",";

                labelText += selectedNumbers[i].Number.ToString();
            }

            LabelActivateData informationLableData = new LabelActivateData(labelText);
            _informationLableRoot.Activate(informationLableData);

            yield return new WaitForSeconds(_data.TimeViewInformationLabel);
            _informationLableRoot.Deactivate();

            yield return new WaitUntil(() => _informationLableRoot.IsComplete);

            foreach (ISelectNumber selectedNumber in selectedNumbers)
            {
                if (CardNumberKeeper.Card.IsSuccessAttack(selectedNumber.Number))
                {
                    SelectResult.SuccessChoice();
                    break;
                }
            }

            //
            string debugMsg = "";

            foreach (ISelectNumber selectNumber in ConfirmableNumbers.SelectedNumbers.OrderByDescending(n => n.Number))
            {
                if (debugMsg != "")
                    debugMsg += ",";

                debugMsg += selectNumber.Number.ToString();
            }

            Debug.Log(debugMsg);
            //

            IsCompleteThis = true;
        }

        private ISelectNumber GetAttackedNumber()
        {
            if (ConfirmableNumbers.SelectedNumbers.Count == _selectNumbers.Length)
            {
                throw new Exception("Не осталось непроверенных (неатакованных) номеров!");
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

                if (ConfirmableNumbers.ContainsSelect(selectNumber) == false)
                {
                    return selectNumber;
                }
            }
            return null;
        }
    }
}