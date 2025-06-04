using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public abstract class CardPlayingZone : MonoBehaviour, ICardDropPlace, IAutomaticFillComponents
    {
        private readonly List<Card> _playedCards = new List<Card>();

        [SerializeField] private RectTransform _rectTransform;
        
        private Table _table;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public bool HasFreeSeat => _table.HasFreeSeat;

        public void Init(Table table)
        {
            _table = table;
            ReadOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
        }

        //public Vector3 GetPosition() => transform.position;

        public void SeatCard(Card card)
        {
            if (HasFreeSeat == false)
                throw new System.Exception("Нет места в " + ToString() + "! Почему не проверил ");

            card.Play();
            _table.SeatCard(card);
            _playedCards.Add(card);
        }

        public IReadOnlyList<Card> DiscardCards()
        {
            List<Card> toDiscard = new List<Card>();
            
            foreach (Card playedCard in _playedCards)
            {
                if (playedCard.TryDiscard())
                {
                    toDiscard.Add(playedCard);
                }
            }
            
            toDiscard = toDiscard.OrderBy(card => card.ReadOnlyRectTransform.GetPositionX()).ToList();

            foreach (Card card in toDiscard)
                _playedCards.Remove(card);

            _table.FreeSeats(toDiscard.Select(card => card));
            
            return toDiscard;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardPlayingZone))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}