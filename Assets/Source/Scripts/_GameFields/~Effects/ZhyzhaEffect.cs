using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public ZhyzhaEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        protected override string GetName() => nameof(ZhyzhaEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Жыжи закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _deactivePerson.ActivateSlimeEffect(_card);
            return UniTask.CompletedTask;
        }
    }
}