using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class FateMistress_FateInevitabilityEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly int _duration;
        private readonly Card _card;

        public FateMistress_FateInevitabilityEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _duration = data.CardEffectData.Duration;
            _card = data.CardEffectData.Card;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Неизбежность судьбы закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _activePerson.ActivateFateInevitability(_card, _duration);
            return UniTask.CompletedTask;
        }
    }
}