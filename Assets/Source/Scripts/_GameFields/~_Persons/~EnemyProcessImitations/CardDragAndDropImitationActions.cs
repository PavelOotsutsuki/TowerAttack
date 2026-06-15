using Cards;
using Tools;
using UnityEngine;
using System.Collections;
using Tools.Utils.Movements;
using GameFields.Persons.Hands;
using GameFields.Persons.DrawCards;
using Cards.DependencyInterlayers;
using GameFields.CardTransits;
using GameFields.Histories;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class CardDragAndDropImitationActions : IEnemyAIObject
    {
        private readonly ICardDragAndDropHandHandler _hand;
        private readonly ICardDropPlace _cardDropPlaceImitation;
        private readonly IAttackable _attackZone;

        private readonly ICardSeatable _discardPile;
        private readonly IDrawCardManager _drawCardManager;
        private readonly ICardSeatable _handPlayer;

        private readonly HistoryRoot _historyRoot;

        private Card _activeCard;
        private ReadOnlyRectTransform _readOnlyCardTransform;
        private Movement _cardMovement;

        private bool _isMoving;

        private bool _isForging;

        public CardDragAndDropImitationActions(ICardDragAndDropHandHandler hand, ICardDropPlace cardDropPlaceImitation, IAttackable attackZone,
            ICardSeatable discardPile, IDrawCardManager drawCardManager, ICardSeatable handPlayer, HistoryRoot historyRoot)
        {
            _hand = hand;
            _cardDropPlaceImitation = cardDropPlaceImitation;
            _attackZone = attackZone;

            _discardPile = discardPile;
            _drawCardManager = drawCardManager;
            _handPlayer = handPlayer;

            _historyRoot = historyRoot;

            _isMoving = false;
        }

        internal void SetCard(Card card)
        {
            _activeCard = card;
            _readOnlyCardTransform = _activeCard.RORTransform;
            _cardMovement = _activeCard.CardMovement;
        }

        public void ViewCard(float duration, float yDirection)
        {
            Vector3 position = _readOnlyCardTransform.GetLocalPosition();
            position.y += _readOnlyCardTransform.GetHeight() / 2 * yDirection;

            _cardMovement.MoveLocalSmoothly(position, Vector3.zero, duration, _activeCard.DefaultScaleVector);
        }

        public void MoveOnPlace(float duration)
        {
            _isMoving = true;

            MoveOnPlace(_cardDropPlaceImitation.RORTransform.GetPosition(), duration);

            _hand.OnCardDrag(_activeCard);
        }

        public bool CanPlay() => _cardDropPlaceImitation.HasFreeSeat;

        public async UniTask Play(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _isMoving == false, cancellationToken: token);

            _hand.OnCardPlay();
            _activeCard.Play();
            //_cardDropPlaceImitation.SeatCard(_activeCard);
        }

        public void HandTransfer()
        {
            _hand.OnCardDrag(_activeCard);
            _hand.OnCardPlay();

            _handPlayer.SeatCard(_activeCard);

            HistoryCardData historyCardData = new HistoryCardData(_activeCard);
            HistoryData historyData = new HistoryData(this, "Передача: ", historyCardData);
            _historyRoot.AddMsg(historyData);
        }

        public async UniTask Forging(CancellationToken token)
        {
            _isForging = false;

            _hand.OnCardDrag(_activeCard);
            _hand.OnCardPlay();

            _discardPile.SeatCard(_activeCard);
            _drawCardManager.DrawCards(1, token, ForgingContinue);

            HistoryCardData historyCardData = new HistoryCardData(_activeCard);
            HistoryData historyData = new HistoryData(this, "Гномичья ковка: ", historyCardData);
            _historyRoot.AddMsg(historyData);

            await UniTask.WaitUntil(() => _isForging, cancellationToken: token);
        }

        private void ForgingContinue()
        {
            _isForging = true;
        }

        public void Attack()
        {
            _hand.OnCardDrag(_activeCard);
            _hand.OnCardPlay();
            //_hand.OnCardAttack();
            _attackZone.Attack(_activeCard);
        }

        //public void ReturnInHand(float returnToHandDuration)
        //{
        //    ReturningInHand(returnToHandDuration).ToUniTask();
        //}

        public async UniTask ReturningInHand(float returnToHandDuration, CancellationToken token)
        {
            await UniTask.WaitUntil(() => _isMoving == false, cancellationToken: token);

            _hand.OnCardDrop();
            _cardMovement.MoveLocalSmoothly(Vector2.zero, Vector3.zero, returnToHandDuration, _activeCard.DefaultScaleVector);
        }

        private void MoveOnPlace(Vector3 position, float duration)
        {
            Vector3 rotation = _readOnlyCardTransform.GetRotationVector();
            Vector3 downWay = position;

            _cardMovement.MoveLinear(downWay, rotation, duration, () => _isMoving = false);
        }

        ////
        ///
        //public void MoveOnAttackPlace(float duration)
        //{
        //    _isMoving = true;

        //    MoveOnPlace(_cardDropPlaceImitation.ReadOnlyRectTransform.GetPosition(), duration);

        //    _hand.OnCardDrag(_activeCard);
        //}
    }
}