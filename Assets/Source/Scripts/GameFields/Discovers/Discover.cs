using System.Collections;
using Cards;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class Discover : MonoBehaviour
    {
        [SerializeField] private float _offset = 400f;
        [SerializeField] private float _positionY = -255f;
        [SerializeField] private DiscoverPanel _discoverPanel;
        [SerializeField] private DiscoverLabel _discoverLabel;
        [SerializeField] private Seat[] _seats;

        private Card[] _cards;

        public void Init()
        {
            _discoverPanel.Init();
            _discoverLabel.Init();

            //_discoverSeatsPool.Init();
            InitSeats();

            gameObject.SetActive(false);
        }

        public void Activate(Card[] cards, string activateMessage)
        {
            _cards = cards;

            SortDiscoverSeats();

            _discoverPanel.Activate();
            _discoverLabel.Activate(activateMessage);

            gameObject.SetActive(true);

            StartCoroutine(PlayingSelection());
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator PlayingSelection()
        {
            yield break;
        }

        private void SortDiscoverSeats()
        {
            float startPositionX;
            Vector3 seatPosition;

            if (_cards.Length % 2 == 1)
            {
                startPositionX = (_cards.Length / 2 * _offset) * -1;
            }
            else
            {
                startPositionX = ((_cards.Length / 2 - 1) * _offset + _offset / 2) * -1;
            }

            for (int i = 0; i < _cards.Length; i++)
            {
                seatPosition = new Vector3(startPositionX + _offset * i, _positionY);

                _seats[i].SetLocalPositionValues(seatPosition, Quaternion.identity.eulerAngles);
            }
        }

        private void InitSeats()
        {
            foreach (Seat seat in _seats)
            {
                seat.Init();
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDiscoverPanel();
            DefineDiscoverLabel();
            DefineSeats();
        }

        [ContextMenu(nameof(DefineDiscoverPanel))]
        private void DefineDiscoverPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private void DefineDiscoverLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeats))]
        private void DefineSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _seats);
        }
    }
}
