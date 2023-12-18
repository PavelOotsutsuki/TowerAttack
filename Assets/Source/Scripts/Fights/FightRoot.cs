using Cards;
using Tools;
using UnityEngine;
using Persons;

namespace Fights
{
    public class FightRoot : MonoBehaviour
    {
        [SerializeField] private FightPVE _fightPVE;

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
