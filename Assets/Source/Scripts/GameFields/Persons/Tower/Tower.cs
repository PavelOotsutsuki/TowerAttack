using Cards;
using GameFields.Persons.Hands;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class Tower : MonoBehaviour, ICardDropPlace
    {
        private const SideType DefaultSideType = SideType.Back;
        private const bool IsCardInteraction = false;

        [SerializeField] private Seat _towerSeat;
        //[SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;

        private IUnbindCardManager _unbindCardManager;

        public bool IsTowerFill => _towerSeat.IsFill();

        public void Init(IUnbindCardManager unbindCardManager)
        {
            _unbindCardManager = unbindCardManager;

            _towerSeat.Init();

            //Deactivate();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public bool TrySeatCard(Card card)
        {
            if (_towerSeat.IsFill() == false)
            {
                card.SetActiveInteraction(IsCardInteraction);
                _towerSeat.SetCard(card, DefaultSideType, _seatDuration);
                _unbindCardManager.UnbindDragableCard();

                return true;
            }

            Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
            return false;
        }

        //private void Deactivate()
        //{
        //    _towerSeat.Disactivate();
        //}

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
