using System.Collections;
using Cards;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.Hands;
using Tools;
using Tools.CommonAnimations;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Towers
{
    public class CardAttackZone : MonoBehaviour, IAttackable
    {
        [SerializeField] private CardAttackZoneData _data;

        private InvertCardAnimation _invertCardAnimation;
        private ShakeAnimation _shakeAnimation;

        private AttackMenu _attackMenu;
        private ReadOnlyRectTransform _towerTransform;

        private DiscardPile _discardPile;

        private Card _card;

        [Inject]
        public void Construct(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        public void Init(AttackMenu attackMenu, ReadOnlyRectTransform towerTransform)
        {
            _attackMenu = attackMenu;
            _towerTransform = towerTransform;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
            _shakeAnimation = new ShakeAnimation(_data.ShakeAnimationConfig);
            //_towerPosition = _towerTransform.GetPosition();
            //_towerSize = _towerTransform.GetRect();
        }

        public void Attack(Card card)
        {
            _card = card;
            //_cardMovement = _card.CardMovement;
            //_cardTransform = _card.ReadOnlyRectTransform;
            //_handBlockable.BlockCards();

            //AttackAnimation attackAnimation = new AttackAnimation(_card.CardMovement, _card.ReadOnlyRectTransform,
            //    _towerTransform.GetPosition(), _towerTransform.GetRect(), _data.AttackAnimationData);

            StartCoroutine(ActivatingAttack());
        }

        private IEnumerator ActivatingAttack()
        {
            AttackAnimation attackAnimation = new AttackAnimation(_card.CardMovement, _card.ReadOnlyRectTransform,
                _towerTransform.GetPosition(), _towerTransform.GetRect(), _data.AttackAnimationData);

            attackAnimation.Play();

            yield return new WaitUntil(() => attackAnimation.IsComplete);

            _shakeAnimation.Play();

            _attackMenu.Activate();

            yield return new WaitUntil(() => _attackMenu.IsComplete);

            //StartCoroutine(Discarding());
            _invertCardAnimation.Play(_card);

            yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

            _discardPile.SeatCard(_card);
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