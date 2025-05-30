using System.Collections.Generic;
using GameFields.Persons.Commons;
using GameFields.Seats;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private GameField _fightPVE;

        public void Init(PersonsState personsState, EnemyAI enemyAI, SignalBus bus, SeatPool seatPool)
        {
            _fightPVE.Init(personsState, enemyAI, bus, seatPool);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(GameFieldRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFightPVE()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFightPVE))]
        private ComponentAttachInfo DefineFightPVE()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _fightPVE, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}