using UnityEngine;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.DrawCards;

namespace GameFields.Effects
{
    public class RushingMailmanEffect : Effect
    {
        private readonly int _countDrawCards = 2;

        private readonly IDrawCardManager _drawCardManager;

        public RushingMailmanEffect(Person activePerson, EffectData data) : base(data)
        {
            _drawCardManager = activePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            bool isContinue = false;

            _drawCardManager?.DrawCards(_countDrawCards, () => isContinue = true);

            yield return new WaitUntil(() => isContinue);
        }

        public override void End()
        {
            base.End();

            Debug.Log("End Несущегося почтальона effect");
        }
    }
}