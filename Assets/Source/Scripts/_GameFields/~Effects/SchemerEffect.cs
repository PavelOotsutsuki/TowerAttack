using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class SchemerEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Person _activePerson;
        private readonly Card _card;

        public SchemerEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        protected override string GetName() => nameof(SchemerEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Шулера закончен");
        //}

        protected override UniTask OnPlaying()
        {
            //_deactivePerson.ActivateDoubleEffect(1);
            //_activePerson.ActivateDoubleEffect(2);
            _deactivePerson.ActivateDoubleEffect(_card);
            _activePerson.ActivateDoubleEffect(_card);
            return UniTask.CompletedTask;
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}