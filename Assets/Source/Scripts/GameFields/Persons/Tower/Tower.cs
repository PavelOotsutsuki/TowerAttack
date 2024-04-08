using Cards;
using GameFields.Persons.Hands;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class Tower : MonoBehaviour, ICardDropSeatPlaceImitation
    {
        private const bool _isFrontCardSide = false;

        [SerializeField] private Seat _towerSeat;
        //[SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;

        private IUnbindCardManager _unbindCardManager;

        public bool IsTowerFill => _towerSeat.IsFill();

        public virtual void Init(IUnbindCardManager unbindCardManager)
        {
            _unbindCardManager = unbindCardManager;

            _towerSeat.Init();

            Deactivate();
        }

        public Vector3 GetCentralСoordinates()
        {
            return transform.position;
        }

        public bool TrySeatCard(ISeatable seatableObject)
        {
            if (_towerSeat.IsFill() == false)
            {
                _towerSeat.SetCard(seatableObject, _isFrontCardSide, _seatDuration);
                _unbindCardManager.UnbindDragableCard();

                return true;
            }

            Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
            return false;
        }

        private void Deactivate()
        {
            _towerSeat.Disactivate();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTowerSeat();
            //DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private void DefineTowerSeat()
        {
            AutomaticFillComponents.DefineComponent(this, ref _towerSeat, ComponentLocationTypes.InChildren);
        }

        //[ContextMenu(nameof(DefineCanvasGroup))]
        //private void DefineCanvasGroup()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        //}
    }
}
