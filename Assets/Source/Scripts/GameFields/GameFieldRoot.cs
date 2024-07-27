using Cards;
using GameFields.Effects;
using GameFields.EndTurnButtons;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour
    {
        [SerializeField] private GameFieldPVE _fightPVE;

        public void Init(PersonsState personsState)
        {
            _fightPVE.Init(personsState);
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
