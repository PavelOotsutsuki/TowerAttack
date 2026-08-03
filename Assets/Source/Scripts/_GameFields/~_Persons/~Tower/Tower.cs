using System;
using System.Collections.Generic;
using System.Threading;
using Cards;
using Cards.Views;
using Cysharp.Threading.Tasks;
using GameFields.Persons.ConfirmableNumbersView;
using Servers;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour, ITowerCardSeatable, ICardNumberKeeper, IBoomTower, IPersonObject, ICopyCardCreator,
        IReadOnlyRectTransformable, ICardFeatureRechangablePlace, ITowerTransitable, IAutomaticFillComponents
    {
        private const SideType DefaultSideType = SideType.Back;
        private const bool IsCardInteraction = false;

        [SerializeField] private TowerSeat _towerSeat;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;
        [SerializeField] private Stone[] _stones;
        [SerializeField] private BoomAnimationData _boomAnimationData;
        [Inject] private FightProcessDBManager _fightProcessDBManager;

        private BoomAnimation _boomAnimation;
        private ConfirmableNumbers _confirmableNumbers;
        private ICardCreator _cardCreator;

        private CancellationToken _fightToken;

        public ReadOnlyRectTransform RORTransform { get; private set; }
        public bool HasFreeSeat => _towerSeat.IsFill() == false;
        public ICardNumber Card => _towerSeat.Card;

        private bool? IsPlayersAction => this is IPlayerObject ? true : this is IEnemyAIObject ? false : null;

        public virtual void Init(ConfirmableNumbers confirmableNumbers, ICardCreator cardCreator, CancellationToken fightToken)
        {
            _towerSeat.Init(_fightProcessDBManager);
            _towerSeat.SetOwner(GetName(), IsPlayersAction);
            _confirmableNumbers = confirmableNumbers;
            _cardCreator = cardCreator;
            _fightToken = fightToken;

            RORTransform = new ReadOnlyRectTransform(_rectTransform);

            BoomAnimationConfig boomAnimationConfig = new BoomAnimationConfig(_towerSeat, _stones, RORTransform
                , _boomAnimationData);

            _boomAnimation = new BoomAnimation(boomAnimationConfig);
        }

        public IEnumerable<IFeatureRechanger> GetRechangableCards()
        {
            if (HasFreeSeat)
                return null;

            return new List<IFeatureRechanger>() { _towerSeat.Card };
        }

        //public virtual void SeatCard(Card card)
        //{
        //    if (HasFreeSeat)
        //    {
        //        card.SetActiveInteraction(IsCardInteraction);
        //        _towerSeat.SetCard(card, DefaultSideType, _seatDuration);
        //    }
        //    else
        //    {
        //        Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
        //    }
        //}

        public virtual void SeatCard(Card card)
        {
            if (HasFreeSeat)
            {
                card.SetActiveInteraction(IsCardInteraction);
                _towerSeat.SetCard(card, DefaultSideType, _seatDuration);
            }
            else
            {
                Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
            }
        }

        protected abstract string GetName();

        bool ITowerTransitable.TryTakeAwayCard(out Card card)
        {
            card = null;

            if (HasFreeSeat)
                return false;

            card = _towerSeat.Card;
            _towerSeat.Reset();
            _confirmableNumbers.Clear();

            return true;
        }

        void ICopyCardCreator.CreateCopyCard(Action<Card> insertedCardCallback)
        {
            CreatingCopyCard(insertedCardCallback, _fightToken).Forget();
        }

        protected CardViewData GetCardViewData()
        {
            return _towerSeat.Card.ViewData;
        }

        void IBoomTower.Boom()
        {
            _boomAnimation.Play(_fightToken);
        }

        private async UniTask CreatingCopyCard(Action<Card> insertedCardCallback, CancellationToken token)
        {
            CardName cardName = _towerSeat.Card.CardName;

            Card createdCard = _cardCreator.CreateCard(cardName, _rectTransform);

            Vector3 localPosition = Vector3.zero;

            createdCard.transform.localPosition = localPosition;
            createdCard.transform.localScale = new Vector3(0,0,0);
            createdCard.transform.rotation = Quaternion.identity;
            createdCard.SetSide(SideType.Back);

            Movement cardMovement = createdCard.CardMovement;
            ReadOnlyRectTransform cardRORTransform = createdCard.RORTransform;

            cardMovement.MoveLocalSmoothly(localPosition, cardRORTransform.GetRotationVector(), 0.5f, createdCard.DefaultScaleVector);

            await UniTask.WaitForSeconds(0.5f + 0.5f, cancellationToken: token);
            insertedCardCallback?.Invoke(createdCard);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Tower))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerSeat(),
                DefineRectTransform(),
                DefineStones()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private ComponentAttachInfo DefineTowerSeat()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _towerSeat, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineStones))]
        private ComponentAttachInfo DefineStones()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _stones);
        }
        #endregion
    }
}