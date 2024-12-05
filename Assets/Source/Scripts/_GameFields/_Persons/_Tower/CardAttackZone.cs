using System.Collections;
using Cards;
using GameFields.DiscardPiles;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.Hands;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Towers
{
    public class CardAttackZone : MonoBehaviour, IAttackable
    {
        [SerializeField] private AttackAnimationData _attackAnimationData;
        [SerializeField] private Transform _cameraTransform;

        private AttackMenu _attackMenu;
        private ReadOnlyRectTransform _towerTransform;

        private DiscardPile _discardPile;

        private Card _card;

        [Inject]
        public void Construct(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        public void Init(AttackMenu attackMenu, IReadOnlyRectTransformable tower)
        {
            _attackMenu = attackMenu;
            _towerTransform = tower.ReadOnlyRectTransform;

            //_towerPosition = _towerTransform.GetPosition();
            //_towerSize = _towerTransform.GetRect();
        }

        public void Attack(Card card)
        {
            _card = card;
            //_cardMovement = _card.CardMovement;
            //_cardTransform = _card.ReadOnlyRectTransform;
            //_handBlockable.BlockCards();

            AttackAnimation attackAnimation = new AttackAnimation(_card.CardMovement, _card.ReadOnlyRectTransform,
                _towerTransform.GetPosition(), _towerTransform.GetRect(), _attackAnimationData);

            attackAnimation.Play();


            StartCoroutine(ActivatingAttack(attackAnimation));
        }

        private IEnumerator ActivatingAttack(AttackAnimation attackAnimation)
        {
            attackAnimation.Play();

            yield return new WaitUntil(() => attackAnimation.IsComplete);

            StartCoroutine(ShakeCamera());

            _attackMenu.Activate();

            yield return new WaitUntil(() => _attackMenu.IsComplete);

            StartCoroutine(Discarding());
        }

        private IEnumerator Discarding()
        {
            Card card = _card;

            InvertCardFront(card);
            yield return new WaitForSeconds(0.5f);

            card.SetSide(SideType.Back);

            InvertCardBack(card);
            yield return new WaitForSeconds(0.5f + 1f);

            _discardPile.SeatCard(card);
        }

        private void InvertCardFront(Card card)
        {
            Vector3 position = card.ReadOnlyRectTransform.GetPosition();

            Movement cardMovement = card.CardMovement;

            cardMovement.MoveLinear(position, new Vector3(0f, -90f, 0f), 0.5f);
        }

        private void InvertCardBack(Card card)
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = card.ReadOnlyRectTransform.GetPosition();

            Movement cardMovement = card.CardMovement;

            cardMovement.MoveSmoothly(position, endRotationVector, 0.5f, card.ReadOnlyRectTransform.GetLocalScale());
        }

        private IEnumerator ShakeCamera()
        {
            float duration = 0.2f;
            Vector3 originalPosition = _cameraTransform.position;

            float x;
            float y;
            float timeLeft = Time.time;

            while ((timeLeft + duration) > Time.time)
            {
                x = Random.Range(-0.3f, 0.3f);
                y = Random.Range(-0.3f, 0.3f);

                _cameraTransform.position = new Vector3(x, y, originalPosition.z);
                yield return new WaitForSeconds(0.025f);
            }

            _cameraTransform.position = originalPosition;
        }
    }
}