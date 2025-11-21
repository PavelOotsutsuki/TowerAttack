using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.DrawCards;
using System.Collections.Generic;

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

        protected override IEnumerator OnPlaying()
        {
            bool isContinue = false;

            List<Card> cards = _drawCardManager?.DrawCards(_countDrawCards, () => isContinue = true);

            foreach (Card card in cards)
            {
                card.SetCurseMode();
            }

            yield return new WaitUntil(() => isContinue);
        }

        public override void End()
        {
            base.End();

            Debug.Log("End Проклятого почтальона effect");
        }
    }
}