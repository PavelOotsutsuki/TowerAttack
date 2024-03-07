using UnityEngine;
using Zenject;
using GameFields;
using GameFields.DiscardPiles;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons;
using Tools;

namespace Installers
{
    public class FightInstaller : MonoInstaller
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private TableAI _tableAI;
        [SerializeField] private TablePlayer _tablePlayer;
        [SerializeField] private HandAI _handAI;
        [SerializeField] private HandPlayer _handPlayer;
        [SerializeField] private TowerAI _towerAI;
        [SerializeField] private TowerPlayer _towerPlayer;
        [SerializeField] private EnemyAnimator _enemyAnimator;

        public override void InstallBindings()
        {
            Container.Bind<Deck>().FromInstance(_deck).AsSingle();
            Container.Bind<DiscardPile>().FromInstance(_discardPile).AsSingle();
            Container.Bind<TableAI>().FromInstance(_tableAI).AsSingle();
            Container.Bind<TablePlayer>().FromInstance(_tablePlayer).AsSingle();
            Container.Bind<HandAI>().FromInstance(_handAI).AsSingle();
            Container.Bind<HandPlayer>().FromInstance(_handPlayer).AsSingle();
            Container.Bind<TowerAI>().FromInstance(_towerAI).AsSingle();
            Container.Bind<TowerPlayer>().FromInstance(_towerPlayer).AsSingle();
            Container.Bind<EnemyAnimator>().FromInstance(_enemyAnimator).AsSingle();
            Container.Bind<EnemyAI>().AsSingle();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineDiscardPile();
            DefineTableAI();
            DefineTablePlayer();
            DefineHandAI();
            DefineHandPlayer();
            DefineTowerAI();
            DefineTowerPlayer();
            DefineEnemyAnimator();
        }

        [ContextMenu(nameof(DefineDeck))]
        private void DefineDeck()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deck, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineDiscardPile))]
        private void DefineDiscardPile()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPile, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineTableAI))]
        private void DefineTableAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tableAI, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineTablePlayer))]
        private void DefineTablePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tablePlayer, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineHandAI))]
        private void DefineHandAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handAI, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineHandPlayer))]
        private void DefineHandPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handPlayer, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineTowerAI))]
        private void DefineTowerAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _towerAI, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineTowerPlayer))]
        private void DefineTowerPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _towerPlayer, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineEnemyAnimator))]
        private void DefineEnemyAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enemyAnimator, ComponentLocationTypes.InScene);
        }
    }
}