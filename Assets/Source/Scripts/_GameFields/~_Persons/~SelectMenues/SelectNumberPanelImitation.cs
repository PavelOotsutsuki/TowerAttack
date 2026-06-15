using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools.UI;
using UnityEngine;
using System.Linq;
using GameFields.Persons.Towers;
using GameFields.Persons.ConfirmableNumbersView;
using System.Threading;
using Tools;
using System.Reflection;

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
            ConfirmableNumbers confirmableNumbers, LastSelectedNumbersWatcher lastSelectedNumbersWatcher,
            CancellationToken fightToken)
        {
            _selectNumbers = new SelectNumberImitation[сardNumbers.Length];

            base.Init(cardNumberKeeper, сardNumbers, selectedNumbers, confirmableNumbers, _selectNumbers,
                lastSelectedNumbersWatcher, fightToken);
        }

        protected override void InitNumbers()
        {
            for (int i = 0; i < CardNumbers.Length; i++)
            {
                SelectNumberImitation selectNumber = new SelectNumberImitation(i + 1);
                _selectNumbers[i] = selectNumber;
            }
        }

        protected override void OnActivate(CancellationToken token)
        {
            Selecting(token).Forget();
        }

        protected override async UniTask Deactivating(CancellationToken token)
        {
            try
            {
                FadablePanel.Hide(new CancellationTokenData(token));

                //yield return new WaitForSeconds(0.1f);
                await UniTask.WaitUntil(() => FadablePanel.IsComplete, cancellationToken: token);

                IsCompleteThis = true;
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask Selecting(CancellationToken token)
        {
            try
            {
                //yield return new WaitForSeconds(0.1f);
                await UniTask.WaitUntil(() => FadablePanel.IsComplete, cancellationToken: token);
                await UniTask.WaitForSeconds(_data.DelayThinkImitation / 2f, cancellationToken: token);
                GameFieldGC.Collect();
                await UniTask.WaitForSeconds(_data.DelayThinkImitation / 2f, cancellationToken: token);

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

                SetSelectResultData setSelectResultData = new SetSelectResultData(resultType, labelText, token);
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
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }
    }
}