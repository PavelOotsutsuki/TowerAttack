using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons;
using GameFields.Persons.DrawCards;

namespace GameFields.Effects
{
    public class RobinGoodEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly Person _deactivePerson;

        private readonly CardLocationViewRoot _cardLocationViewRoot;

        public RobinGoodEffect(Person deactivePerson, CardLocationViewRoot cardLocationViewRoot,
            EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;

            _cardLocationViewRoot = cardLocationViewRoot;

            Play();
        }

        protected override string GetName() => nameof(RobinGoodEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Робин Гуда закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            Person player;
            Person enemyAI;

            if (_activePerson is Player)
            {
                player = _activePerson;
                enemyAI = _deactivePerson;
            }
            else
            {
                player = _deactivePerson;
                enemyAI = _activePerson;
            }

            int countHandAI = _cardLocationViewRoot.GetAllCards(ViewType.HandAI).Count();
            int countHandPlayer = _cardLocationViewRoot.GetAllCards(ViewType.HandPlayer).Count();
            int countCards = countHandPlayer - countHandAI;

            if (countCards == 0)
            {
                return;
            }

            IDrawCardManager gettedPerson;

            if (countCards > 0)
            {
                gettedPerson = enemyAI;
            }
            else
            {
                countCards *= -1;
                gettedPerson = player;
            }

            bool isDraw = false;
            gettedPerson.DrawCards(countCards, Token, () => isDraw = true);

            await UniTask.WaitUntil(() => isDraw, cancellationToken: Token);
        }
    }
}