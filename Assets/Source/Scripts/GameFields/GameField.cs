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
        private FightStepsController _fightStepsController;

        public void Init(PersonsState personsState)
        {
            _startTowerCardSelection.Init(personsState);

            FightResult fightResult = new FightResult();
            Fight fight = new Fight(personsState, fightResult);
            EndFight endFight = new EndFight(fightResult);
            _fightStepsController = new FightStepsController(_startTowerCardSelection, fight, endFight);

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
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelection, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
