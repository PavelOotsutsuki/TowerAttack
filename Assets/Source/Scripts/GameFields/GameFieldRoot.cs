using Cards;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour
    {
        [SerializeField] private GameFieldPVE _fightPVE;

        public void Init(Card[] cards)
        {
            InitFightPVE(cards);
        }

        private void InitFightPVE(Card[] cards)
        {
            _fightPVE.Init(cards);
        }

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
    }
}
