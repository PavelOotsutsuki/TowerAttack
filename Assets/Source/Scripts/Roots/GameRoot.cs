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
        [SerializeField] private Player _player;
        [SerializeField] private EnemyAI _enemyAI;

        private void Start()
        {
            InitAll();
        }

        private void InitAll()
        {
            InitCardRoot();
            InitPlayer();
            InitEnemyAI();
            InitGameFieldRoot(_cardRoot.Cards);
        }

        private void InitCardRoot()
        {
            _cardRoot.Init();
        }

        private void InitPlayer()
        {
            _player.Init();
        }

        private void InitEnemyAI()
        {
            _enemyAI.Init();
        }

        private void InitGameFieldRoot(Card[] cards)
        {
            _gameFieldRoot.Init(cards, _player, _enemyAI);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardRoot();
            DefineGameFieldRoot();
            DefinePlayer();
            DefineEnemyAI();
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

        [ContextMenu(nameof(DefinePlayer))]
        private void DefinePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _player, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineEnemyAI))]
        private void DefineEnemyAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enemyAI, ComponentLocationTypes.InThis);
        }
    }
}