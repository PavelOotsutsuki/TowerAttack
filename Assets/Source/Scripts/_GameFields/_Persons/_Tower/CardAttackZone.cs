using System.Collections;
using Cards;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons.AttackMenues;
using GameFields.Signals;
using Tools;
using Tools.CommonAnimations;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Towers
{
    public abstract class CardAttackZone : MonoBehaviour, IAttackable, ICompletable, IPersonObject, IAttackResultHandler
    {
        [SerializeField] private CardAttackZoneData _data;

        private InvertCardAnimation _invertCardAnimation;
        private ShakeAnimation _shakeAnimation;

        private IAttackMenu _attackMenu;
        private IBoomTower _tower;

        private DiscardPile _discardPile;
        protected SignalBus Bus;

        private Card _currentCard;

        private bool _isComplete;

        public bool IsComplete => _isComplete;

        [Inject]
        public void Construct(DiscardPile discardPile, SignalBus bus)
        {
            _discardPile = discardPile;
            Bus = bus;

            _isComplete = false;
        }

        public void Init(IAttackMenu attackMenu, IBoomTower tower)
        {
            _attackMenu = attackMenu;
            _tower = tower;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            _shakeAnimation = new ShakeAnimation(_data.ShakeAnimationConfig);
        }

        public void Attack(Card card)
        {
            _isComplete = false;

            AttackProcessingActivate();

            StartCoroutine(ActivatingAttack(card));
        }

        protected abstract void AttackProcessingActivate();

        private IEnumerator ActivatingAttack(Card card)
        {
            _currentCard = card;

            ReadOnlyRectTransform towerTransform = _tower.ReadOnlyRectTransform;

            AttackAnimation attackAnimation = new AttackAnimation(card.CardMovement, card.ReadOnlyRectTransform,
                towerTransform.GetPosition(), towerTransform.GetRect(), _data.AttackAnimationData);

            attackAnimation.Play();

            yield return new WaitUntil(() => attackAnimation.IsComplete);

            _shakeAnimation.Play();

            AttackMenuActivateData attackMenuActivateData = new AttackMenuActivateData(_data.NeedSelectForAttack);

            _attackMenu.Activate(attackMenuActivateData);
        }

        void IAttackResultHandler.SuccessAttack()
        {
            StartCoroutine(SuccessAttackProcessing());
        }

        void IAttackResultHandler.FalledAttack()
        {
            StartCoroutine(FalledAttackProcessing());
        }

        private IEnumerator FalledAttackProcessing()
        {
            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard);

                yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

                _discardPile.SeatCard(_currentCard);

                _currentCard = null;
            }

            _isComplete = true;
        }

        private IEnumerator SuccessAttackProcessing()
        {
            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard);

                yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

                _discardPile.SeatCard(_currentCard);

                _currentCard = null;
            }

            _tower.Boom();

            yield return new WaitForSeconds(_data.DelayBeforeStartingEndFightActions);

            Bus.Fire(new PersonWinSignal(this));
        }
    }
}