using System.Collections.Generic;
using System.Threading;
using Cards;
using GameFields.Persons.ConfirmableNumbersView;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerAI : Tower, IEnemyAIObject
    {
        [SerializeField] private TowerEnemyHelper _towerEnemyHelper;

        public void Init(ConfirmableNumbersViewRoot confirmableNumbersViewRoot, ConfirmableNumbers confirmableNumbers, ICardCreator cardCreator, CancellationToken fightToken)
        {
            base.Init(confirmableNumbers, cardCreator, fightToken);

            _towerEnemyHelper.Init(confirmableNumbersViewRoot);
        }

        public override void SeatCard(Card card)
        {
            if (HasFreeSeat)
            {
                _towerEnemyHelper.Activate();
            }
            else
            {
                _towerEnemyHelper.Deactivate();
            }

            base.SeatCard(card);
        }

        protected override string GetName() => nameof(TowerAI);

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerAI))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerEnenyHelper()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineTowerEnenyHelper))]
        private ComponentAttachInfo DefineTowerEnenyHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _towerEnemyHelper, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}