using UnityEngine;
using GameFields.StartFights;
using GameFields.Effects;
using GameFields.Persons;
using Tools.Utils.FillComponents;
using System.Collections.Generic;
using Zenject;
using GameFields.EndFights;

namespace GameFields
{
    public class GameField : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private StartFight _startFight;
        [SerializeField] private EndFight _endFight;

        private EffectFactory _effectFactory;
        private FightStepsController _fightStepsController;

        public void Init(PersonsState personsState, Player player, EnemyAI enemyAI, SignalBus bus)
        {
            _startFight.Init(player, enemyAI);

            FightResult fightResult = new FightResult();
            Fight fight = new Fight(personsState, fightResult, bus);
            _endFight.Init(fightResult);
            _fightStepsController = new FightStepsController(_startFight, fight, _endFight);

            //_fightStepsController.NextStep();
            _fightStepsController.StartStep();
        }

        //private void Update()
        //{
        //    _fightStepsController.Update();
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(GameField))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartFight(),
                DefineEndFight()
            };

            return list;
        }

        [ContextMenu(nameof(DefineStartFight))]
        private ComponentAttachInfo DefineStartFight()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _startFight, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndFight))]
        private ComponentAttachInfo DefineEndFight()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _endFight, ComponentLocationTypes.InScene);
        }
        #endregion 
    }
}