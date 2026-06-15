using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ImpArmyEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public ImpArmyEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Армии чертей закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _deactivePerson.AddCurse(_card);
            return UniTask.CompletedTask;
        }
    }
}