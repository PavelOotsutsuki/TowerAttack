using System.Collections.Generic;
using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour, ICardDropPlace, ICardNumberKeeper, IBoomTower, IPersonObject, IReadOnlyRectTransformable, IAutomaticFillComponents
    {
        private const SideType DefaultSideType = SideType.Back;
        private const bool IsCardInteraction = false;

        [SerializeField] private TowerSeat _towerSeat;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;
        [SerializeField] private Stone[] _stones;
        [SerializeField] private BoomAnimationData _boomAnimationData;

        private BoomAnimation _boomAnimation;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public bool HasFreeSeat => _towerSeat.IsFill() == false;
        public ICardNumber Card => _towerSeat.Card;

        public void Init()
        {
            _towerSeat.Init();

            ReadOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);

            BoomAnimationConfig boomAnimationConfig = new BoomAnimationConfig(_towerSeat, _stones, ReadOnlyRectTransform
                , _boomAnimationData);

            _boomAnimation = new BoomAnimation(boomAnimationConfig);
        }

        public void SeatCard(Card card)
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

        void IBoomTower.Boom()
        {
            _boomAnimation.Play();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Tower))]
        public List<ComponentAttachInfo> DefineAllComponents()
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