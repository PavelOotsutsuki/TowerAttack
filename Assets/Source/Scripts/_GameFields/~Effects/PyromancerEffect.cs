using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class PyromancerEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public PyromancerEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Пироманта закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _deactivePerson.ActivateFireDraw(_card);
            return UniTask.CompletedTask;
        }
    }
}