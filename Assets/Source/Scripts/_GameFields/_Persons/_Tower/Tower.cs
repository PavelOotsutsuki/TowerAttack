using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Seats;
using GameFields.Signals;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour, ICardDropPlace, ICardNumberKeeper, IReadOnlyRectTransformable, IPersonObject, IAutomaticFillComponents
    {
        private const SideType DefaultSideType = SideType.Back;
        private const bool IsCardInteraction = false;

        [SerializeField] protected Seat TowerSeat;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;

        private SignalBus _bus;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public bool HasFreeSeat => TowerSeat.IsFill() == false;
        public ICardNumber Card => TowerSeat.Card;

        public void Init(SignalBus bus)
        {
            Debug.Log("tower init");
            _bus = bus;

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
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerSeat(),
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private ComponentAttachInfo DefineTowerSeat()
        {
           return AutomaticFillComponents.DefineComponent(this, ref TowerSeat, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}