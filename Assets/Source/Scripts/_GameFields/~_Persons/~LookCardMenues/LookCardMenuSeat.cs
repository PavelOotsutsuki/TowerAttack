using System.Collections.Generic;
using Cards;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeat : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private LookCardMenuCard _lookCardMenuCard;

        private Card _card;

        private Movement _seatMovement;

        public void Init(BigCardRoot bigCardRoot, CardCapabilityDescription cardCapabilityDescription)
        {
            _seatMovement = new Movement(_rectTransform);
            _lookCardMenuCard.Init(bigCardRoot, cardCapabilityDescription);
            Reset();
        }

        public void SetCard(Card card, Vector2? sizeDelta = null)
        {
            Reset();

            sizeDelta ??= _card.RORTransform.GetSizeDelta();

            _card = card;
            LookCardMenuCardActivateData data = new LookCardMenuCardActivateData(sizeDelta.Value, _card.ViewData, _card.CardToken);
            _lookCardMenuCard.Activate(data);
        }

        public void Reset()
        {
            _card = null;
            _lookCardMenuCard.Deactivate();
        }

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LookCardMenuSeat))]
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
            return AutomaticFillComponents.DefineComponent(this, ref _lookCardMenuCard, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}