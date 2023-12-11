using UnityEngine;
using Cards;
using GameFields;
using Tools;
using Persons;

namespace Roots
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private CardRoot _cardRoot;
        [SerializeField] private GameFieldRoot _gameFieldRoot;

        private void Start()
        {
            InitAll();
        }

        private void InitAll()
        {
            InitCardRoot();
            InitGameFieldRoot(_cardRoot.Cards);
        }

        private void InitCardRoot()
        {
            _cardRoot.Init();
        }

        private void InitGameFieldRoot(Card[] cards)
        {
            _gameFieldRoot.Init(cards);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardRoot();
            DefineGameFieldRoot();
        }

        [ContextMenu(nameof(DefineCardRoot))]
        private void DefineCardRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineGameFieldRoot))]
        private void DefineGameFieldRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _gameFieldRoot, ComponentLocationTypes.InThis);
        }
    }
}