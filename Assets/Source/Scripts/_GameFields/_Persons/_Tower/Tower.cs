using System.Collections.Generic;
using Cards;
using GameFields.Persons.Commons;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour, ITowerCardSeatable, ICardNumberKeeper, IBoomTower, IPersonObject,
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

        public ReadOnlyRectTransform RORTransform { get; private set; }
        public bool HasFreeSeat => _towerSeat.IsFill() == false;
        public ICardNumber Card => _towerSeat.Card;

        public virtual void Init(ConfirmableNumbers confirmableNumbers)
        {
            _towerSeat.Init();
            _confirmableNumbers = confirmableNumbers;

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

        protected CardViewData GetCardViewData()
        {
            return _towerSeat.Card.ViewData;
        }

        void IBoomTower.Boom()
        {
            _boomAnimation.Play();
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