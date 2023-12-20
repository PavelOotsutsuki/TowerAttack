using UnityEngine;
using Cards;
using Fights;
using Tools;

namespace Roots
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private CardRoot _cardRoot;
        [SerializeField] private FightRoot _fightRoot;

        private void Start()
        {
            InitAll();
        }

        private void InitAll()
        {
            InitCardRoot();
            InitFightRoot(_cardRoot.Cards);
        }

        private void InitCardRoot()
        {
            _cardRoot.Init();
        }

        private void InitFightRoot(Card[] cards)
        {
            _fightRoot.Init(cards);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardRoot();
            DefineFightRoot();
        }

        [ContextMenu(nameof(DefineCardRoot))]
        private void DefineCardRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFightRoot))]
        private void DefineFightRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fightRoot, ComponentLocationTypes.InThis);
        }
    }
}