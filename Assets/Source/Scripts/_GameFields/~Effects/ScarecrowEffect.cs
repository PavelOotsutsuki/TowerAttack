using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ScarecrowEffect : Effect
    {
        private const int CountUsed = 1;

        private readonly Person _deactivePerson;
        private readonly Card _card;

        public ScarecrowEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        protected override string GetName() => nameof(ScarecrowEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Чучела закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _deactivePerson.ActivateScarecrowEffect(CountUsed, _card);
            return UniTask.CompletedTask;
        }
    }
}