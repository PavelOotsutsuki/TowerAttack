using System;
using Tools;
using UnityEngine;
using GameFields.StartFights;
using GameFields.Effects;
using GameFields.Persons;

namespace GameFields
{
    public class GameField : MonoBehaviour
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

            _fightStepsController.NextStep();
        }

        private void Update()
        {
            _fightStepsController.Update();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFirstTurn();
        }

        [ContextMenu(nameof(DefineFirstTurn))]
        private void DefineFirstTurn()
        {
            AutomaticFillComponents.DefineComponent(this, ref _startFight, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
