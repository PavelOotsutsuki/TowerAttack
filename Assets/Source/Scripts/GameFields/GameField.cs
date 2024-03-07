using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;
using GameFields.FirstTurns;
using Zenject;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private FightAnimator _fightAnimator;
        [SerializeField] private FirstTurn _firstTurn;

        private Deck _deck;
        private DiscardPile _discardPile;
        private EnemyAI _enemyAI;
        private Fight _fight;
        private CardEffects _cardEffects;

        [Inject]
        private void Construct(EnemyAI enemyAI, Deck deck, DiscardPile discardPile)
        {
            _enemyAI = enemyAI;
            _deck = deck;
            _discardPile = discardPile;
        }

        public void Init(Card[] cardsInDeck)
        {
            _firstTurn.Init();
            _deck.Init(cardsInDeck);
            _discardPile.Init();
            _fightAnimator.Init(_discardPile);

            _cardEffects = new CardEffects(_deck, _discardPile);
            _fight = new Fight(_player, _enemyAI, _deck, _discardPile, _endTurnButton, _fightAnimator, _firstTurn);

            _player.Init(_fight, _cardEffects);
            _enemyAI.Init(_fight, _cardEffects);
            _endTurnButton.Init(_fight);

            _fight.StartFirstTurn();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineEndTurnButton();
            DefinePlayer();
            DefineDiscardPile();
            DefineFightAnimator();
            DefineFirstTurn();
        }

        [ContextMenu(nameof(DefineDeck))]
        private void DefineDeck()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deck, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private void DefineEndTurnButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePlayer))]
        private void DefinePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _player, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscardPile))]
        private void DefineDiscardPile()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPile, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightAnimator))]
        private void DefineFightAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fightAnimator, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFirstTurn))]
        private void DefineFirstTurn()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurn, ComponentLocationTypes.InChildren);
        }
    }
}
