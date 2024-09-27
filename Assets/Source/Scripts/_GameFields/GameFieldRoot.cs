using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour
    {
        [SerializeField] private GameField _fightPVE;

        public void Init(PersonsState personsState, Player player, EnemyAI enemyAI)
        {
            _fightPVE.Init(personsState, player, enemyAI);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFightPVE();
        }

        [ContextMenu(nameof(DefineFightPVE))]
        private void DefineFightPVE()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fightPVE, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
