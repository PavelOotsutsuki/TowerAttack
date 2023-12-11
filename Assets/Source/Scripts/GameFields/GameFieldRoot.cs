using Cards;
using Tools;
using UnityEngine;
using Persons;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour
    {
        [SerializeField] private GameFieldPVE _gameFieldPVE;

        public void Init(Card[] cards)
        {
            InitGameFieldPVE(cards);
        }

        private void InitGameFieldPVE(Card[] cards)
        {
            _gameFieldPVE.Init(cards);
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
