using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class AttackNumberPanelEnemyAI : AttackNumberPanel
    {
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

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => FadablePanel.IsComplete);

            IsCompleteThis = true;
        }

        private IEnumerator Attacking()
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => FadablePanel.IsComplete);
            yield return new WaitForSeconds(8f); // Типа думает

            IAttackNumber attackedNumber = GetAttackedNumber() ?? throw new Exception("Ошибка нахождения номера для имитации атаки");

            LabelActivateData informationLableData = new LabelActivateData("Противник выбрал номер: " + attackedNumber.Number);
            _informationLableRoot.Activate(informationLableData);

            yield return new WaitForSeconds(4f);
            _informationLableRoot.Deactivate();

            yield return new WaitUntil(() => _informationLableRoot.IsComplete);

            if (CardNumberKeeper.Card.IsSuccessAttack(attackedNumber.Number))
            {
                AttackResult.SuccessChoice();
            }
            else
            {
                ConfirmableNumbers.Add(attackedNumber);
            }

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

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberPanel))]
        //public List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineFadablePanel()//,
        //        //DefineRectTransform(),
        //        //DefineAttackNumbers()
        //    };

        //    return list;
        //}

        //[ContextMenu(nameof(DefineFadablePanel))]
        //private ComponentAttachInfo DefineFadablePanel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        //}

        ////[ContextMenu(nameof(DefineRectTransform))]
        ////private ComponentAttachInfo DefineRectTransform()
        ////{
        ////    return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        ////}

        ////[ContextMenu(nameof(DefineAttackNumbers))]
        ////private ComponentAttachInfo DefineAttackNumbers()
        ////{
        ////    return AutomaticFillComponents.DefineComponent(this, ref _attackNumbers);
        ////}

        //#endregion
    }
}