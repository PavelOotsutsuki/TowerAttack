using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Views;
using GameFields.Persons.ConfirmableNumbersView;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;

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

        private BoomAnimation _boomAnimation;
        private ConfirmableNumbers _confirmableNumbers;
        private ICardCreator _cardCreator;

        public ReadOnlyRectTransform RORTransform { get; private set; }
        public bool HasFreeSeat => _towerSeat.IsFill() == false;
        public ICardNumber Card => _towerSeat.Card;

        public virtual void Init(ConfirmableNumbers confirmableNumbers, ICardCreator cardCreator)
        {
            _towerSeat.Init();
            _confirmableNumbers = confirmableNumbers;
            _cardCreator = cardCreator;

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
            StartCoroutine(CreatingCopyCard(insertedCardCallback));
        }

        protected CardViewData GetCardViewData()
        {
            return _towerSeat.Card.ViewData;
        }

        void IBoomTower.Boom()
        {
            _boomAnimation.Play();
        }

        private IEnumerator CreatingCopyCard(Action<Card> insertedCardCallback)
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

            yield return new WaitForSeconds(0.5f + 0.5f);
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