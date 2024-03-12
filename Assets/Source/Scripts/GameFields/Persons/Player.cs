using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons.PersonAnimators;
using Tools;
using UnityEngine;
using System.Collections.Generic;
using GameFields.Effects;

namespace GameFields.Persons
{
    internal class Player : MonoBehaviour, IPerson
    {
        [SerializeField] private HandPlayer _hand;
        [SerializeField] private TablePlayer _table;
        [SerializeField] private TowerPlayer _tower;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform _transform;

        public int CountDrawCards => _playerAnimator.CountDrawCards;
        public float DrawCardsDelay => _playerAnimator.DrawCardsDelay;

        public void Init(IStartFightListener startFightListener, EffectRoot effectRoot)
        {
            _hand.Init();
            InitTower(startFightListener);
            _table.Init(this, effectRoot);
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
            DefineHandPlayer();
            DefineTablePlayer();
            DefineTowerPlayer();
            DefinePlayerAnimator();
            DefineTransform();
        }

        [ContextMenu(nameof(DefineHandPlayer))]
        private void DefineHandPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _hand, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTablePlayer))]
        private void DefineTablePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _table, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTowerPlayer))]
        private void DefineTowerPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tower, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePlayerAnimator))]
        private void DefinePlayerAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _playerAnimator, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
    }
}
