using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons.Towers;
using Tools;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers
{
    public class FalsePrinceEffectHandler
    {
        private readonly ICopyCardCreator _towerCopyCardCreator;
        private readonly ICardSeatable _deck;

        public FalsePrinceEffectHandler(ICopyCardCreator towerCopyCardCreator, ICardSeatable deck)
        {
            _towerCopyCardCreator = towerCopyCardCreator;
            _deck = deck;
        }

        public void Activate(CallbackHandler callbackHandler)
        {
            Activating(callbackHandler).ToUniTask();
        }

        private IEnumerator Activating(CallbackHandler callbackHandler)
        {
            Card createdCard = null; 

            _towerCopyCardCreator.CreateCopyCard((card) => createdCard = card);

            yield return new WaitUntil(() => createdCard != null);
            yield return new WaitForSeconds(1f);

            _deck.SeatCard(createdCard);
            yield return new WaitForSeconds(0.2f);

            callbackHandler.Complete();
        }
    }
}