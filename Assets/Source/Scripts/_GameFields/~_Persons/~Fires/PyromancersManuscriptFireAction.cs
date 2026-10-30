using System.Threading;
using Cards;
using Cards.Views;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class PyromancersManuscriptFireAction
    {
        private readonly ICardCreator _cardCreator;
        private readonly SideType _actionCardSideType;
        private readonly Transform _parent;
        private readonly CancellationToken _fightToken;

        public PyromancersManuscriptFireAction(ICardCreator cardCreator, SideType actionCardSideType, Transform parent, CancellationToken fightToken)
        {
            _cardCreator = cardCreator;
            _actionCardSideType = actionCardSideType;
            _parent = parent;
            _fightToken = fightToken;
        }

        public void Play(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler)
        {
            CreatePyromants(card, cardSeatable, index, callbackHandler, _fightToken).Forget();
        }

        private async UniTask CreatePyromants(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler, CancellationToken token)
        {
            Vector3 worldPosition = card.RORTransform.GetPosition();
            Vector3 localScale = card.RORTransform.GetLocalScale();
            Quaternion rotation = card.transform.rotation;

            Card card1 = CreateCard(worldPosition, localScale, rotation, _parent);
            Card card2 = CreateCard(worldPosition, localScale, rotation, _parent);

            cardSeatable.SeatCard(card1, index);
            cardSeatable.SeatCard(card2, index + 1);

            await UniTask.WaitForSeconds(1f, cancellationToken: token);
            callbackHandler.Complete();
        }

        private Card CreateCard(Vector3 worldPosition, Vector3 localScale, Quaternion rotation, Transform parent)
        {
            Card card = _cardCreator.CreateCard(CardName.Pyromancer, parent);

            SideType sideType = _actionCardSideType;

            card.transform.position = worldPosition;
            card.transform.localScale = localScale;
            card.transform.rotation = rotation;
            card.SetSide(sideType);

            return card;
        }

        #region OldCreatePyromants
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
        #endregion
    }
}