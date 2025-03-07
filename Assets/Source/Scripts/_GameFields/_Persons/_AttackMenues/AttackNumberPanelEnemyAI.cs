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

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class AttackNumberPanelEnemyAI : AttackNumberPanel
    {
        [SerializeField] private AttackNumberPanelEnemyAIData _data;

        private AttackNumberImitation[] _attackNumbers;

        private InformationLableRoot _informationLableRoot;

        [Inject]
        public void Construct(InformationLableRoot informationLableRoot)
        {
            _informationLableRoot = informationLableRoot;
            _informationLableRoot.Init();
        }

        protected override void InitNumbers()
        {
            _attackNumbers = new AttackNumberImitation[CountNumbers];

            for (int i = 0; i < CountNumbers; i++)
            {
                AttackNumberImitation attackNumber = new AttackNumberImitation(i + 1);
                _attackNumbers[i] = attackNumber;
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

            List<IAttackNumber> selectedNumbers = new List<IAttackNumber>();

            for (int i = 0; i < NeedForActivate; i++)
            {
                IAttackNumber attackedNumber = GetAttackedNumber() ?? throw new Exception("Ошибка нахождения номера для имитации атаки");
                ConfirmableNumbers.Add(attackedNumber);
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

            foreach (IAttackNumber selectedNumber in selectedNumbers)
            {
                if (CardNumberKeeper.Card.IsSuccessAttack(selectedNumber.Number))
                {
                    AttackResult.SuccessChoice();
                    break;
                }
            }

            //
            string debugMsg = "";

            foreach (IAttackNumber attackNumber in ConfirmableNumbers.AcceptNumbers.OrderByDescending(n => n.Number))
            {
                if (debugMsg != "")
                    debugMsg += ",";

                debugMsg += attackNumber.Number.ToString();
            }

            Debug.Log(debugMsg);
            //

            IsCompleteThis = true;
        }

        private IAttackNumber GetAttackedNumber()
        {
            if (ConfirmableNumbers.AcceptNumbers.Count == _attackNumbers.Length)
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
                int attackNumber = allNumbers[Random.Range(0, allNumbers.Count)];
                shuffleNumbers.Add(attackNumber);
                allNumbers.Remove(attackNumber);
            }
            foreach (int number in shuffleNumbers)
            {
                IAttackNumber attackNumber = _attackNumbers[number - 1];

                if (ConfirmableNumbers.Contains(attackNumber) == false)
                {
                    return attackNumber;
                }
            }
            return null;
        }
    }
}