using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverSeat : MonoBehaviour, IDiscoverClickHandler
    {
        [SerializeField] protected DiscoverCard DiscoverCard;
        [SerializeField] private RectTransform _rectTransform;

        protected Card Card;

        private Movement _seatMovement;
        private IDiscoverChoiceHandler _discoverChoiceHandler;

        public void Init(IDiscoverChoiceHandler discoverChoiceHandler)
        {
            _seatMovement = new Movement(_rectTransform);
            _discoverChoiceHandler = discoverChoiceHandler;
            DiscoverCard.Init(OnDiscoverCardClick, this);
            Reset();
        }

        public abstract void SetCard(Card card);

        public void StartClick()
        {
            DiscoverCard.StartClickActions();
        }

        public void Reset()
        {
            Card = null;
            DiscoverCard.Hide();
        }

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        private void OnDiscoverCardClick()
        {
            _discoverChoiceHandler.OnMakeChoice(Card);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
            DefineDiscoverCard();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineDiscoverCard))]
        private void DefineDiscoverCard()
        {
            AutomaticFillComponents.DefineComponent(this, ref DiscoverCard, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}
