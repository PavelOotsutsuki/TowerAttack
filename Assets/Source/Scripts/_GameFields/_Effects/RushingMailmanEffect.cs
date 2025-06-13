using UnityEngine;
using Cards;
using GameFields.Persons.Commons;
using System.Collections;
using GameFields.Persons.DrawCards;

namespace GameFields.Effects
{
    public class RushingMailmanEffect : Effect
    {
        private readonly int _countDrawCards = 2;

        private readonly IDrawCardManager _drawCardManager;

        private bool _isContinue;

        public RushingMailmanEffect(Person activePerson) : base()
        {
            _drawCardManager = activePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            _isContinue = false;

            _drawCardManager?.DrawCards(_countDrawCards, Continue);

            yield return new WaitUntil(() => _isContinue);
        }

        private void Continue()
        {
            _isContinue = true;
        }


        public override void End()
        {
            //Debug.Log("End patriarch corall effect");
        }
    }
}