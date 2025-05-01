using System.Collections;
using Cards;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Signals;
using Tools;
using Tools.CommonAnimations;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Towers
{
    public abstract class CardAttackZone : MonoBehaviour, IAttackable, IPersonObject, IAttackCardKeeper
    {
        [SerializeField] private CardAttackZoneData _data;

        //private InvertCardAnimation _invertCardAnimation;
        private ShakeAnimation _shakeAnimation;

        private ISelectMenuActivator _attackMenu;
        private IReadOnlyRectTransformable _tower;

        //private DiscardPile _discardPile;
        protected SignalBus Bus;
        protected ICompletable AttackHandler;

        private Card _currentCard;

        //private bool _isComplete;

        //public bool IsComplete => _isComplete;

        //[Inject]
        //public void Construct(DiscardPile discardPile, SignalBus bus)
        //{
        //    _discardPile = discardPile;
        //    Bus = bus;

        //    _isComplete = false;
        //}

        public Card SeizeAttackingCard
        {
            get
            {
                Card currentCard = _currentCard;

                _currentCard = null;
                return currentCard;
            }
        }

        public void Init(ISelectMenuActivator attackMenu, IReadOnlyRectTransformable tower, SignalBus bus,
            ICompletable attackHandler)
        {
            _attackMenu = attackMenu;
            _tower = tower;
            Bus = bus;
            AttackHandler = attackHandler;

            //_invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            _shakeAnimation = new ShakeAnimation(_data.ShakeAnimationConfig);
        }

        public void Attack(Card card)
        {
            //_isComplete = false;
            _currentCard = card;

            AttackProcessingActivate();

            StartCoroutine(ActivatingAttack(card));
        }

        protected abstract void AttackProcessingActivate();

        private IEnumerator ActivatingAttack(Card card)
        {
            //_currentCard = card;

            ReadOnlyRectTransform towerTransform = _tower.ReadOnlyRectTransform;

            AttackAnimation attackAnimation = new AttackAnimation(card.CardMovement, card.ReadOnlyRectTransform,
                towerTransform.GetPosition(), towerTransform.GetRect(), _data.AttackAnimationData);

            attackAnimation.Play();

            yield return new WaitUntil(() => attackAnimation.IsComplete);

            _shakeAnimation.Play();

            SelectMenuActivateData attackMenuActivateData = new SelectMenuActivateData(_data.NeedSelectForAttack);
            //AttackMenuActivateData attackMenuActivateData = new AttackMenuActivateData(49);

            _attackMenu.Activate(attackMenuActivateData);
        }

        //void ISelectResultHandler.SetResult(SetSelectResultData data)
        //{
        //    switch (data.ResultType)
        //    {
        //        case ResultType.Success:
        //            StartCoroutine(SuccessAttackProcessing());
        //            break;
        //        case ResultType.Falled:
        //            StartCoroutine(FalledAttackProcessing());
        //            break;
        //        default:
        //            throw new System.Exception("Неизвестный ResultType");
        //    }
        //}

        //private IEnumerator FalledAttackProcessing()
        //{
        //    if (_currentCard is not null)
        //    {
        //        _invertCardAnimation.Play(_currentCard);

        //        yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

        //        _discardPile.SeatCard(_currentCard);

        //        _currentCard = null;
        //    }

        //    _isComplete = true;
        //}

        //private IEnumerator SuccessAttackProcessing()
        //{
        //    if (_currentCard is not null)
        //    {
        //        _invertCardAnimation.Play(_currentCard);

        //        yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

        //        _discardPile.SeatCard(_currentCard);

        //        _currentCard = null;
        //    }

        //    _tower.Boom();

        //    yield return new WaitForSeconds(_data.DelayBeforeStartingEndFightActions);

        //    Bus.Fire(new PersonWinSignal(this));
        //}
    }
}