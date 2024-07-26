using UnityEngine;
using Cards;
using GameFields;
using Tools;

namespace Roots
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private CardRoot _cardRoot;
        [SerializeField] private GameFieldRoot _gameFieldRoot;
        [SerializeField] private ScreenRoot _screenRoot;

        private void Start()
        {
            _screenRoot.Init();
            _cardRoot.Init();
            _gameFieldRoot.Init(_cardRoot.Cards);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardRoot();
            DefineGameFieldRoot();
            DefineScreenRoot();
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

        [ContextMenu(nameof(DefineScreenRoot))]
        private void DefineScreenRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _screenRoot, ComponentLocationTypes.InThis);
        }
    }
}