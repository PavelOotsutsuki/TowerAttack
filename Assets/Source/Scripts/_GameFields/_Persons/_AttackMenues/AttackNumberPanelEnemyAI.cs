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
        [SerializeField] private FadablePanel _fadablePanel;

        private AttackNumberImitation[] _attackNumbers;

        private ICardNumberKeeper _cardNumberKeeper;
        private AttackResult _attackResult;
        private InformationLableRoot _informationLableRoot;

        private ConfirmableNumbers _confirmableNumbers;
        private int _countNumbers;

        private bool _isComplete;

        public override bool IsComplete => _isComplete && _fadablePanel.IsComplete;

        public override bool? IsActive { get; protected set; }

        [Inject]
        public void Construct(InformationLableRoot informationLableRoot)
        {
            _informationLableRoot = informationLableRoot;
            _informationLableRoot.Init();
        }

        public void Init(ICardNumberKeeper cardNumberKeeper, int countNumbers)
        {
            _cardNumberKeeper = cardNumberKeeper;
            _countNumbers = countNumbers;
            _attackResult = null;

            _confirmableNumbers = new ConfirmableNumbers();

            InitNumbers();

            _fadablePanel.Init();
        }

        public override void Activate(AttackNumberPanelActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _isComplete = false;


            _attackResult = data.AttackResult;
            gameObject.SetActive(true);

            _fadablePanel.Show();
            Attacking().ToUniTask();
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            _isComplete = false;

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            _fadablePanel.Hide();

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _isComplete = true;
        }

        private IEnumerator Attacking()
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => _fadablePanel.IsComplete);
            yield return new WaitForSeconds(8f); // Типа думает

            IAttackNumber attackedNumber = GetAttackedNumber() ?? throw new Exception("Ошибка нахождения номера для имитации атаки");

            LabelActivateData informationLableData = new LabelActivateData("Противник выбрал номер: " + attackedNumber.Number);
            _informationLableRoot.Activate(informationLableData);

            yield return new WaitForSeconds(4f);
            _informationLableRoot.Deactivate();

            yield return new WaitUntil(() => _informationLableRoot.IsComplete);

            if (_cardNumberKeeper.Card.IsSuccessAttack(attackedNumber.Number))
            {
                _attackResult.SuccessChoice();
            }
            else
            {
                _confirmableNumbers.Add(attackedNumber);
            }

            _isComplete = true;
        }

        private IAttackNumber GetAttackedNumber()
        {
            if (_confirmableNumbers.AcceptNumbers.Count == _attackNumbers.Length)
            {
                throw new Exception("Не осталось непроверенных (неатакованных) номеров!");
            }

            List<int> shuffleNumbers = new List<int>();
            List<int> allNumbers = new List<int>();

            for (int i = 0; i < _countNumbers; i++)
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

                if (_confirmableNumbers.Contains(attackNumber) == false)
                {
                    return attackNumber;
                }
            }

            return null;
        }

        private void InitNumbers()
        {
            _attackNumbers = new AttackNumberImitation[_countNumbers];

            for (int i = 0; i < _countNumbers; i++)
            {
                AttackNumberImitation attackNumber = new AttackNumberImitation(i + 1);
                _attackNumbers[i] = attackNumber;
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberPanel))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}