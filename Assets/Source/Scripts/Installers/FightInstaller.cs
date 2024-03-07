using UnityEngine;
using Zenject;
using GameFields;
using GameFields.DiscardPiles;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons;

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
    }
}