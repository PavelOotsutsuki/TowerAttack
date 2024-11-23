using Cards;
using GameFields.Seats;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour, ICardDropPlace, ICardNumberKeeper, IAutomaticFillComponents
    {
        private const SideType DefaultSideType = SideType.Back;
        private const bool IsCardInteraction = false;

        [SerializeField] protected Seat TowerSeat;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public bool HasFreeSeat => TowerSeat.IsFill() == false;
        public ICardNumber Card => TowerSeat.Card;

        public void Init()
        {
            TowerSeat.Init();
            ReadOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
        }

        //public Vector3 GetPosition() => transform.position;

        public void SeatCard(Card card)
        {
            if (HasFreeSeat)
            {
                card.SetActiveInteraction(IsCardInteraction);
                TowerSeat.SetCard(card, DefaultSideType, _seatDuration);
            }
            else
            {
                Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Tower))]
        public void DefineAllComponents()
        {
            DefineTowerSeat();
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private void DefineTowerSeat()
        {
            AutomaticFillComponents.DefineComponent(this, ref TowerSeat, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}