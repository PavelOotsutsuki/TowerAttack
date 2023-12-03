using Cards;
using GameFields.Tables;
using GameFields.Hands;
using GameFields.FightProcess;
using Tools;
using UnityEngine;
using Persons;

namespace GameFields
{
    public class GameField : MonoBehaviour//, IPlayCardManager//, IEndTurnHandler
    {
        [SerializeField] private TablePlayer _tablePlayer;
        [SerializeField] private TableAI _tableAI;
        [SerializeField] private HandPlayer _handPlayer;
        [SerializeField] private HandAI _handAI;
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private DrawCardAnimator _drawCardAnimator;

        private Fight _fight;

        public void Init(Card[] cardsInDeck, Player player, EnemyAI enemyAI)
        {
            _fight = new Fight(player, enemyAI, _handPlayer, _handAI, _tablePlayer, _tableAI, _deck, _drawCardAnimator);

            InitTables();
            InitDeck(cardsInDeck);
            InitHands();
            InitEndTurnButton();
        }

        //public void PlayCard(Card card)
        //{
        //    _handPlayer.RemoveCard(card);
        //}

        //public void OnEndTurn()
        //{
        //    if (_deck.TryTakeCard(out Card drawnCard))
        //    {
        //        drawnCard.AddToHand(_handPlayer);
        //        _drawCardAnimator.Init(_handPlayer, drawnCard);
        //    }
        //}

        private void InitHands()
        {
            _handPlayer.Init(HandOwner.Player);
            _handAI.Init(HandOwner.Enemy);
        }

        private void InitTables()
        {
            _tablePlayer.Init(_fight);
            _tableAI.Init(_fight);
        }

        private void InitDeck(Card[] cards)
        {
            _deck.Init(cards);
        }

        private void InitEndTurnButton()
        {
            _endTurnButton.Init(_fight);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTablePlayer();
            DefineHandPlayer();
            DefineTableAI();
            DefineHandAI();
            DefineDeck();
            DefineEndTurnButton();
            DefineDrawCardAnimator();
        }

        [ContextMenu(nameof(DefineTablePlayer))]
        private void DefineTablePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tablePlayer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineHandPlayer))]
        private void DefineHandPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handPlayer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTableAI))]
        private void DefineTableAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tableAI, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineHandAI))]
        private void DefineHandAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handAI, ComponentLocationTypes.InChildren);
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

        [ContextMenu(nameof(DefineDrawCardAnimator))]
        private void DefineDrawCardAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _drawCardAnimator, ComponentLocationTypes.InThis);
        }
    }
}
