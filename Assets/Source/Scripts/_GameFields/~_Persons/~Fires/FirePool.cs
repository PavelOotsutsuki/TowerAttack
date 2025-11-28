using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Views;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons.Hands;
using Tools;
using Tools.Settings;
using Tools.Utils.Screens;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class FirePool : IFirePoolSeatable
    {
        private const float CenterRotation = 90f;

        private readonly float _maxCoordinateX;
        private readonly float _maxCoordinateY;
        private readonly float _minCoordinateX;
        private readonly float _minCoordinateY;
        private readonly float _cardRotationOffset = 30f;

        private readonly List<Card> _fireList;
        private readonly Transform _parent;
        private readonly ICardCreator _cardCreator;

        public FirePool(Transform parent, ICardCreator cardCreator)
        {
            _fireList = new List<Card>();
            _parent = parent;
            _cardCreator = cardCreator;

            _maxCoordinateX = ((RectTransform)parent).rect.width / 2f;
            _maxCoordinateY = ((RectTransform)parent).rect.height / 2f;
            _minCoordinateX = _maxCoordinateX * -1;
            _minCoordinateY = _maxCoordinateY * -1;
        }

        public IReadOnlyList<Card> FireList => _fireList;
        public int Count => _fireList.Count;

        //public void SeatCard(Card card, int index = -1)
        //{
        //    if (index < 0 || index > _fireList.Count)
        //        index = _fireList.Count;

        //    card.gameObject.SetActive(false);
        //    card.ResetDrag();

        //    Seat(card);

        //    _fireList.Insert(index, card);
        //}

        public void SeatCard(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler)
        {
            card.gameObject.SetActive(false);
            card.ResetDrag();

            if (card.IsPyromancersManuscript)
            {
                CreatePyromants(card, cardSeatable, index, callbackHandler).ToUniTask();
            }
            else
            {
                callbackHandler.Complete();
            }

            Seat(card);

            _fireList.Add(card);
        }

        public int IndexOf(Card card)
        {
           return _fireList.IndexOf(card);
        }

        public void Remove(Card card)
        {
            card.Rise();

            _fireList.Remove(card);
        }

        public void Clear()
        {
            _fireList.Clear();
        }

        //private IEnumerator CreatePyromants(Card card, ICardSeatable cardSeatable, int index)
        //{
        //    Vector3 localScale = card.RORTransform.GetLocalScale();

        //    float factorX = ScreenView.GetFactorX();
        //    float factorY = ScreenView.GetFactorY();

        //    Vector3 worldPosition = card.RORTransform.GetPosition();
        //    Vector2 cardSize = new Vector2(GameSettings.CardSize.x * factorX, GameSettings.CardSize.y * factorY) * localScale;

        //    Debug.Log($"worldPosition: {worldPosition}");
        //    Debug.Log($"localScale: {localScale}");
        //    Debug.Log($"cardSize: {cardSize}");
        //    Debug.Log($"factorX: {factorX}");
        //    Debug.Log($"factorY: {factorY}");


        //    float xOffset = cardSize.x / 5f; // Чтобы увеличить расстояние между спамящимися пиромантами
        //    //float xOffset = 0; // Чтобы увеличить расстояние между спамящимися пиромантами
        //    Debug.Log($"xOffset: {xOffset}");
        //    float yVector = worldPosition.y < ScreenView.Y() * factorY / 2f? 1f : -1f; // Если позиция меньше половины, спамим сверху, иначе снизу
        //    float yPosition = worldPosition.y + cardSize.y / 2f * yVector;
        //    float xBetweenCardsOffset = cardSize.x / 2f + xOffset;

        //    Card card1 = _cardCreator.CreateCard(CardName.Pyromancer, _parent);

        //    card1.transform.position = new Vector3(worldPosition.x - xBetweenCardsOffset, yPosition, worldPosition.z);
        //    card1.transform.localScale = localScale;
        //    card1.SetSide(SideType.Front);
        //    Debug.Log($"card1.transform.position = {card1.transform.position}");
        //    Card card2 = _cardCreator.CreateCard(CardName.Pyromancer, _parent);

        //    card2.transform.position = new Vector3(worldPosition.x + xBetweenCardsOffset, yPosition, worldPosition.z);
        //    card2.transform.localScale = localScale;
        //    card2.SetSide(SideType.Front);
        //    Debug.Log($"card2.transform.position = {card2.transform.position}");

        //    yield return new WaitForSeconds(1f);

        //    if (cardSeatable is HandAI)
        //    {
        //        InvertCardAnimationData invertCardAnimationData1 = new InvertCardAnimationData(0.5f, 0.5f, 0.5f, false, SideType.Front);
        //        InvertCardAnimation invertCardAnimation1 = new InvertCardAnimation(invertCardAnimationData1);
        //        invertCardAnimation1.Play(card1);

        //        InvertCardAnimationData invertCardAnimationData2 = new InvertCardAnimationData(0.5f, 0.5f, 0.5f, false, SideType.Front);
        //        InvertCardAnimation invertCardAnimation2 = new InvertCardAnimation(invertCardAnimationData2);
        //        invertCardAnimation2.Play(card2);

        //        yield return new WaitUntil(() => invertCardAnimation1.IsComplete && invertCardAnimation2.IsComplete);
        //    }
        //    else
        //    {
        //        yield return new WaitForSeconds(1f);
        //    }

        //    cardSeatable.SeatCard(card1, index);
        //    cardSeatable.SeatCard(card2, index+1);
        //}

        private IEnumerator CreatePyromants(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler)
        {
            Vector3 localScale = card.RORTransform.GetLocalScale();
            Quaternion rotation = card.transform.rotation;
            Vector3 worldPosition = card.RORTransform.GetPosition();

            Card card1 = _cardCreator.CreateCard(CardName.Pyromancer, _parent);

            SideType sideType = cardSeatable is HandAI ? SideType.Back : SideType.Front;

            //card1.transform.position = new Vector3(worldPosition.x - xBetweenCardsOffset, yPosition, worldPosition.z);
            card1.transform.position = worldPosition;
            card1.transform.localScale = localScale;
            card1.transform.rotation = rotation;
            card1.SetSide(sideType);
            Card card2 = _cardCreator.CreateCard(CardName.Pyromancer, _parent);

            card2.transform.position = worldPosition;
            card2.transform.localScale = localScale;
            card2.transform.rotation = rotation;
            card2.SetSide(sideType);

            cardSeatable.SeatCard(card1, index);
            cardSeatable.SeatCard(card2, index + 1);

            yield return new WaitForSeconds(1f);
            callbackHandler.Complete();
            yield break;
        }

        private void Seat(Card card)
        {
            card.SetActiveInteraction(false);
            card.RORTransform.SetParent(_parent);
            card.CardMovement.MoveLocalInstantly(FindCardSeatPosition(), FindCardSeatRotation());
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
    }
}