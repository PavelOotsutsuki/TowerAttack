using Cards;
using Tools;
using UnityEngine;
using Persons;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour
    {
        [SerializeField] private GameFieldPVE _gameFieldPVE;

        public void Init(Card[] cards, Player player, EnemyAI enemyAI)
        {
            InitGameFieldPVE(cards, player, enemyAI);
        }

        private void InitGameFieldPVE(Card[] cards, Player player, EnemyAI enemyAI)
        {
            _gameFieldPVE.Init(cards, player, enemyAI);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineGameFieldPVE();
        }

        [ContextMenu(nameof(DefineGameFieldPVE))]
        private void DefineGameFieldPVE()
        {
            AutomaticFillComponents.DefineComponent(this, ref _gameFieldPVE, ComponentLocationTypes.InChildren);
        }
    }
}
