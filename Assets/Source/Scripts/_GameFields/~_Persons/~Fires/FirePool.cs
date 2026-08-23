using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Cards;
using GameFields.CardTransits;
using GameFields.Histories;
using Servers;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public abstract class FirePool : IFirePoolSeatable, IPersonObject
    {
        private const float CenterRotation = 90f;

        private readonly float _maxCoordinateX;
        private readonly float _maxCoordinateY;
        private readonly float _minCoordinateX;
        private readonly float _minCoordinateY;
        private readonly float _cardRotationOffset = 30f;

        private readonly List<Card> _fireList;
        private readonly Transform _parent;
        private readonly ExtraFireSeatActionRoot _extraFireSeatActionRoot;
        private readonly HistoryRoot _historyRoot;
        private readonly FightProcessDBManager _fightProcessDBManager;

        public FirePool(Transform parent, ExtraFireSeatActionRoot extraFireSeatActionRoot, HistoryRoot historyRoot, FightProcessDBManager fightProcessDBManager)
        {
            _fireList = new List<Card>();
            _parent = parent;
            _extraFireSeatActionRoot = extraFireSeatActionRoot;
            _historyRoot = historyRoot;
            _fightProcessDBManager = fightProcessDBManager;

            _maxCoordinateX = ((RectTransform)parent).rect.width / 2f;
            _maxCoordinateY = ((RectTransform)parent).rect.height / 2f;
            _minCoordinateX = _maxCoordinateX * -1;
            _minCoordinateY = _maxCoordinateY * -1;
        }

        public IReadOnlyList<Card> FireList => _fireList;
        public int Count => _fireList.Count;

        private bool? IsPlayersAction => this is IPlayerObject ? true : this is IEnemyAIObject ? false : null;

        public void SeatCard(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler)
        {
            card.gameObject.SetActive(false);
            card.ResetDrag();

            _extraFireSeatActionRoot.Play(card, cardSeatable, index, callbackHandler);

            Seat(card);


            //string cardName = card.CurrentSide == SideType.Front ? card.Name.ToUpper() : "?";

            HistoryData historyData = new HistoryData(this, "Сожжена карта: ", new HistoryCardData(card));
            _historyRoot.AddMsg(historyData);
        }

        public int IndexOf(Card card)
        {
           return _fireList.IndexOf(card);
        }

        public void Remove(Card card)
        {
            card.Rise();

            _fireList.Remove(card);

            //string cardName = card.CurrentSide == SideType.Front ? card.Name.ToUpper() : "?";
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, card.ViewData.Number.ToString(), "RISE", GetName());
            WriteFullListIntoDB();
            HistoryData historyData = new HistoryData(this, "Восстановлена карта: ", new HistoryCardData(card));
            _historyRoot.AddMsg(historyData);
        }

        public void Clear()
        {
            _fireList.Clear();
            WriteFullListIntoDB();
        }

        private void Seat(Card card)
        {
            card.SetActiveInteraction(false);
            card.RORTransform.SetParent(_parent);
            card.CardMovement.MoveLocalInstantly(FindCardSeatPosition(), FindCardSeatRotation());
            _fireList.Add(card);
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, card.ViewData.Number.ToString(), "FIRE", GetName());
            WriteFullListIntoDB();
        }

        private Vector3 FindCardSeatPosition()
        {
            float xCoordinate = Random.Range(_minCoordinateX, _maxCoordinateX);
            float yCoordinate = Random.Range(_minCoordinateY, _maxCoordinateY);

            return new Vector3(xCoordinate, yCoordinate, 0f);
        }

        private Vector3 FindCardSeatRotation()
        {
            float zRotation = Random.Range(CenterRotation - _cardRotationOffset, CenterRotation + _cardRotationOffset);

            return new Vector3(0f, 0f, zRotation);
        }

        private void WriteFullListIntoDB()
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, GetSerializedCards(), "FULLLIST", GetName());
        }

        private string GetSerializedCards()
        {
            return JsonSerializer.Serialize(_fireList.Select(c => c.ViewData.Number));
        }

        protected abstract string GetName();
    }
}