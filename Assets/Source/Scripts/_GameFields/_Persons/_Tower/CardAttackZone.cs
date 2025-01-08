using System.Collections;
using Cards;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.Hands;
using GameFields.Signals;
using Tools;
using Tools.CommonAnimations;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Towers
{
    public abstract class CardAttackZone : MonoBehaviour, IAttackable, ICompletable, IPersonObject, IAttackResultHandler
    {
        [SerializeField] private CardAttackZoneData _data;

        private InvertCardAnimation _invertCardAnimation;
        private ShakeAnimation _shakeAnimation;

        private AttackMenu _attackMenu;
        private IReadOnlyRectTransformable _towerTransformable;

        private DiscardPile _discardPile;
        private SignalBus _bus;

        private Card _currentCard;

        private bool _isComplete;

        public bool IsComplete => _isComplete;

        [Inject]
        public void Construct(DiscardPile discardPile, SignalBus bus)
        {
            _discardPile = discardPile;
            _bus = bus;

            _isComplete = false;
        }

        public void Init(AttackMenu attackMenu, IReadOnlyRectTransformable towerTransformable)
        {
            _attackMenu = attackMenu;
            _towerTransformable = towerTransformable;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            _shakeAnimation = new ShakeAnimation(_data.ShakeAnimationConfig);
            //_towerPosition = _towerTransform.GetPosition();
            //_towerSize = _towerTransform.GetRect();
        }

        public void Attack(Card card)
        {
            _isComplete = false;
            //_cardMovement = _card.CardMovement;
            //_cardTransform = _card.ReadOnlyRectTransform;
            //_handBlockable.BlockCards();

            //AttackAnimation attackAnimation = new AttackAnimation(_card.CardMovement, _card.ReadOnlyRectTransform,
            //    _towerTransform.GetPosition(), _towerTransform.GetRect(), _data.AttackAnimationData);
            _bus.Fire(new AttackSignal(this));

            StartCoroutine(ActivatingAttack(card));
        }

        private IEnumerator ActivatingAttack(Card card)
        {
            _currentCard = card;

            ReadOnlyRectTransform towerTransform = _towerTransformable.ReadOnlyRectTransform;

            AttackAnimation attackAnimation = new AttackAnimation(card.CardMovement, card.ReadOnlyRectTransform,
                towerTransform.GetPosition(), towerTransform.GetRect(), _data.AttackAnimationData);

            attackAnimation.Play();

            yield return new WaitUntil(() => attackAnimation.IsComplete);

            _shakeAnimation.Play();

            _attackMenu.Activate();

            //yield return new WaitUntil(() => _attackMenu.IsComplete);

            ////StartCoroutine(Discarding());
            //_invertCardAnimation.Play(card);

            //yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

            //_discardPile.SeatCard(card);

            //_isComplete = true;
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
            Debug.Log("Мимо!");

            _invertCardAnimation.Play(_currentCard);

            yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

            _discardPile.SeatCard(_currentCard);

            _isComplete = true;
        }

        private IEnumerator SuccessAttackProcessing()
        {
            Debug.Log("Победа!");

            yield return new WaitForSeconds(5f);

            _bus.Fire(new PersonWinSignal(this));
        }

        //private IEnumerator Discarding()
        //{
        //    Card card = _card;

        //    InvertCardFront(card);
        //    yield return new WaitForSeconds(0.5f);

        //    card.SetSide(SideType.Back);

        //    InvertCardBack(card);
        //    yield return new WaitForSeconds(0.5f + 1f);

        //    _discardPile.SeatCard(card);
        //}

        //private void InvertCardFront(Card card)
        //{
        //    Vector3 position = card.ReadOnlyRectTransform.GetPosition();

        //    Movement cardMovement = card.CardMovement;

        //    cardMovement.MoveLinear(position, new Vector3(0f, -90f, 0f), 0.5f);
        //}

        //private void InvertCardBack(Card card)
        //{
        //    Vector3 endRotationVector = Vector3.zero;
        //    Vector3 position = card.ReadOnlyRectTransform.GetPosition();

        //    Movement cardMovement = card.CardMovement;

        //    cardMovement.MoveSmoothly(position, endRotationVector, 0.5f, card.ReadOnlyRectTransform.GetLocalScale());
        //}

        //private IEnumerator ShakeCamera()
        //{
        //    float duration = 0.2f;
        //    Vector3 originalPosition = _cameraTransform.position;

        //    float x;
        //    float y;
        //    float timeLeft = Time.time;

        //    while ((timeLeft + duration) > Time.time)
        //    {
        //        x = Random.Range(-0.3f, 0.3f);
        //        y = Random.Range(-0.3f, 0.3f);

        //        _cameraTransform.position = new Vector3(x, y, originalPosition.z);
        //        yield return new WaitForSeconds(0.025f);
        //    }

        //    _cameraTransform.position = originalPosition;
        //}
    }
}