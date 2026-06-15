using Cards;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class CursedMailmanEffect : Effect
    {
        private readonly int _countDrawCards = 2;

        private readonly IDrawCardManager _drawCardManager;

        public CursedMailmanEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _drawCardManager = deactivePerson;

            Play();
        }

        protected override async UniTask OnPlaying()
        {
            bool isContinue = false;

            List<Card> cards = _drawCardManager?.DrawCards(_countDrawCards, Token, () => isContinue = true);

            foreach (Card card in cards)
            {
                card.SetCurseMode();
            }

            await UniTask.WaitUntil(() => isContinue, cancellationToken: Token);
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("End Проклятого почтальона effect");
        //}
    }
}