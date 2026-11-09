using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons.Towers;
using Tools;

namespace GameFields.Persons.EffectHandlers
{
    public class FalsePrinceEffectHandler
    {
        private readonly ICopyCardCreator _towerCopyCardCreator;
        private readonly ICardSeatable _deck;
        private readonly CancellationToken _fightToken;

        public FalsePrinceEffectHandler(ICopyCardCreator towerCopyCardCreator, ICardSeatable deck, CancellationToken fightToken)
        {
            _towerCopyCardCreator = towerCopyCardCreator;
            _deck = deck;
            _fightToken = fightToken;
        }

        public void Activate(CallbackHandler callbackHandler)
        {
            Activating(callbackHandler, _fightToken).Forget();
        }

        private async UniTask Activating(CallbackHandler callbackHandler, CancellationToken token)
        {
            Card createdCard = null; 

            _towerCopyCardCreator.CreateCopyCard((card) => createdCard = card);

            await UniTask.WaitUntil(() => createdCard != null, cancellationToken: token);
            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            _deck.SeatCard(createdCard);
            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);

            callbackHandler.Complete();
        }
    }
}