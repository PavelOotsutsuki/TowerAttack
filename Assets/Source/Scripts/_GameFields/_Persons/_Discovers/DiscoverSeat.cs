using System.Collections.Generic;
using Cards;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverSeat : MonoBehaviour, IDiscoverClickHandler, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private DiscoverCard _discoverCard;

        private Card _card;

        private Movement _seatMovement;
        private IDiscoverChoiceHandler _discoverChoiceHandler;

        public void Init(IDiscoverChoiceHandler discoverChoiceHandler)
        {
            _seatMovement = new Movement(_rectTransform);
            _discoverChoiceHandler = discoverChoiceHandler;
            _discoverCard.Init(OnDiscoverCardClick, this);
            Reset();
        }

        public void SetCard(Card card)
        {
            _card = card;
            DiscoverCardActivateData data = new DiscoverCardActivateData(_card.ReadOnlyRectTransform.GetSizeDelta(), _card.ViewConfig);
            _discoverCard.Activate(data);
        }

        public void StartClick()
        {
            _discoverCard.StartClickActions();
        }

        public void Reset()
        {
            _card = null;
            _discoverCard.Deactivate();
        }

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        private void OnDiscoverCardClick()
        {
            _discoverChoiceHandler.OnMakeChoice(_card);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DiscoverSeat))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineDiscoverCard()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineDiscoverCard))]
        private ComponentAttachInfo DefineDiscoverCard()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _discoverCard, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}