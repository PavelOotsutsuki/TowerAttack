using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons.PersonAnimators;
using Tools;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace GameFields.Persons
{
    internal class Player : MonoBehaviour, IPerson
    {
        [SerializeField] private Transform _transform;

        private HandPlayer _hand;
        private TablePlayer _table;
        private TowerPlayer _tower;
        private PlayerAnimator _playerAnimator;

        public int CountDrawCards => _playerAnimator.CountDrawCards;
        public float DrawCardsDelay => _playerAnimator.DrawCardsDelay;

        [Inject]
        public void Construct(HandPlayer hand, TablePlayer table, TowerPlayer tower, PlayerAnimator playerAnimator)
        {
            _hand = hand;
            _table = table;
            _tower = tower;
            _playerAnimator = playerAnimator;
        }

        public void Init(IStartFightListener startFightListener, CardEffects cardEffects)
        {
            _hand.Init();
            InitTower(startFightListener);
            _table.Init(this, cardEffects);
//            cardEffects.SetPlayerGameFieldElements(_table, _hand, _tower);

            _playerAnimator.Init(_hand, _transform);
        }

        public List<Card> GetDiscardCards()
        {
            return _table.GetDiscardCards();
        }

        public void PlayCard(Card card)
        {
            _hand.RemoveCard(card);
        }

        public void ActivateDropPlaces()
        {
            _table.Activate();
            _tower.Activate();
        }

        public void DeactivateDropPlaces()
        {
            _table.Deactivate();
            _tower.Deactivate();
        }

        public void DrawCard(Card card)
        {
            card.SetDragAndDropListener(_hand);
            _playerAnimator.StartDrawCardAnimation(card);
        }

        private void InitTower(IStartFightListener startFightListener)
        {
            _tower.Init(this);
            _tower.SetStartFightListener(startFightListener);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTransform();
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
    }
}
