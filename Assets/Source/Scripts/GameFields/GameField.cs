using Tools;
using UnityEngine;
using GameFields.StartTowerCardSelections;
using GameFields.Effects;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private StartTowerCardSelection _startTowerCardSelection;

        private EffectFactory _effectFactory;

        public void Init(PersonsState personsState)
        {
            _startTowerCardSelection.Init(personsState);

            FightResult fightResult = new FightResult();
            Fight fight = new Fight(personsState, fightResult);
            EndFight endFight = new EndFight(fightResult);
            FightStepsController fightStepsController = new FightStepsController(_startTowerCardSelection, fight, endFight);

            fightStepsController.StartStep();
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
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelection, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
