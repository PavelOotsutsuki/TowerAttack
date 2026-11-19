using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class FateMistress_FatesInevitabilityEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly int _duration;
        private readonly Card _card;

        public FateMistress_FatesInevitabilityEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _duration = data.CardEffectData.Duration;
            _card = data.CardEffectData.Card;

            Play();
        }

        protected override string GetName() => nameof(FateMistress_FatesInevitabilityEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Неизбежность судьбы закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _activePerson.ActivateFatesInevitability(_card, _duration);
            return UniTask.CompletedTask;
        }
    }
}