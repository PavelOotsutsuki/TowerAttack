using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.Towers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    public class SelectMenuImitation : SelectMenu
    {
        [SerializeField] private SelectNumberPanelImitation _selectNumberPanelImitation;
        [SerializeField] private SelectMenuImitationData _data;

        public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler selectResultHandler, int[] cardNumbers,
            SelectNumbersList selectedNumbers, ConfirmableNumbers confirmableNumbers,
            LastSelectedNumbersWatcher lastSelectedNumbersWatcher, CancellationToken fightToken)
        {
            _selectNumberPanelImitation.Init(cardNumberKeeper, cardNumbers, selectedNumbers, confirmableNumbers,
                lastSelectedNumbersWatcher, fightToken);

            SelectMenuLabelTextLogic selectMenuLabelTextLogic = new EnemySelectMenuLabelTextLogic(_data.SelectMenuLabelText);

            base.Init(selectResultHandler, _selectNumberPanelImitation, selectMenuLabelTextLogic, fightToken);
        }

        public override void Activate(SelectMenuActivateData activateData)
        {
            base.Activate(activateData);

            WaitingUntilDeactivate(CurrentCTS.Token).Forget();
        }

        protected override async UniTask OnDeactivating(CancellationToken token)
        {
            try
            {
                await UniTask.WaitForSeconds(_data.DelayAfterChoiceNumberDone, cancellationToken: token);

                _selectNumberPanelImitation.Deactivate();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask WaitingUntilDeactivate(CancellationToken token)
        {
            try
            {
                await UniTask.WaitUntil(() => _selectNumberPanelImitation.IsComplete, cancellationToken: token);

                Deactivate();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectMenuImitation))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineSelectNumberPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineSelectNumberPanel))]
        private ComponentAttachInfo DefineSelectNumberPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectNumberPanelImitation, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}