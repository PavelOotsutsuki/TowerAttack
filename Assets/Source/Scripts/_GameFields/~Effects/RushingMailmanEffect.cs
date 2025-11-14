using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.DrawCards;
using Zenject;

namespace GameFields.Effects
{
    public class RushingMailmanEffect : Effect
    {
        private readonly int _countDrawCards = 2;

        private readonly IDrawCardManager _drawCardManager;

        private bool _isContinue;

        public RushingMailmanEffect(Person activePerson, EffectData data) : base(data)
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
            base.End();

            Debug.Log("End Несущегося почтальона effect");
        }
    }
}