using Cards;
using Tools;

namespace GameFields.Persons.Fires
{
    public class ExtraFireSeatActionRoot
    {
        private readonly PyromancersManuscriptFireAction _pyromancersManuscriptFireAction;

        public ExtraFireSeatActionRoot(PyromancersManuscriptFireAction pyromancersManuscriptFireAction)
        {
            _pyromancersManuscriptFireAction = pyromancersManuscriptFireAction;
        }

        public void Play(Card card, ICardSeatable cardSeatable, int index, CallbackHandler callbackHandler)
        {
            if (card.IsPyromancersManuscript)
            {
                _pyromancersManuscriptFireAction.Play(card, cardSeatable, index, callbackHandler);
                return;
            }

            callbackHandler.Complete();
        }
    }
}