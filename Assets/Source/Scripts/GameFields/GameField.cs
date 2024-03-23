using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.EndTurnButtons;
using GameFields.StartTowerCardSelections;
using GameFields.Effects;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private FightAnimator _fightAnimator;
        [SerializeField] private StartTowerCardSelection _startTowerCardSelection;

        [SerializeField] private EnemyAI _enemyAI;

        //private EffectRoot _effectRoot;
        //private Fight _fight;
        //private FightStepsController _fightStepsController;

        public void Init(Card[] cardsInDeck)
        {
            _startTowerCardSelection.Init(_player, _enemyAI);
            _deck.Init(cardsInDeck);
            _discardPile.Init();
            _fightAnimator.Init(_discardPile);

            Fight fight = new Fight(_player, _enemyAI, _endTurnButton, _fightAnimator);
            EffectRoot effectRoot = new EffectRoot(_deck, _discardPile, fight);
            EndFight endFight = new EndFight();

            _player.Init(effectRoot, _deck, ActivateEndTurnButton);
            _enemyAI.Init(fight, _deck, effectRoot);

            _endTurnButton.Init(fight);
            FightStepsController fightStepsController = new FightStepsController(_startTowerCardSelection, fight, endFight);

            fightStepsController.StartFightSteps();
        }

        private void ActivateEndTurnButton()
        {
            _endTurnButton.SetActiveSide();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineDiscardPile();
            DefineEndTurnButton();
            DefinePlayer();
            DefineFightAnimator();
            DefineFirstTurn();
        }

        [ContextMenu(nameof(DefineDeck))]
        private void DefineDeck()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deck, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscardPile))]
        private void DefineDiscardPile()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPile, ComponentLocationTypes.InChildren);
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

        [ContextMenu(nameof(DefineFightAnimator))]
        private void DefineFightAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fightAnimator, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFirstTurn))]
        private void DefineFirstTurn()
        {
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelection, ComponentLocationTypes.InChildren);
        }
    }
}
