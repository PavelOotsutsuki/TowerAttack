using UnityEngine;
using GameFields.StartFights;
using GameFields.Effects;
using GameFields.Persons;
using Tools.Utils.FillComponents;
using System.Collections.Generic;

namespace GameFields
{
    public class GameField : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private StartFight _startFight;

        private EffectFactory _effectFactory;
        private FightStepsController _fightStepsController;

        public void Init(PersonsState personsState, Player player, EnemyAI enemyAI)
        {
            _startFight.Init(player, enemyAI);

            FightResult fightResult = new FightResult();
            Fight fight = new Fight(personsState, fightResult);
            EndFight endFight = new EndFight(fightResult);
            _fightStepsController = new FightStepsController(_startFight, fight, endFight);

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
                DefineFirstTurn()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFirstTurn))]
        private ComponentAttachInfo DefineFirstTurn()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _startFight, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}