using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class DeadOgreEffect : Effect
    {
        private const int CountTurns = 3;

        private readonly Person _deactivePerson;

        public DeadOgreEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        protected override string GetName() => nameof(DeadOgreEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Мертвого огра закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _deactivePerson.AddCurse(CountTurns);

            return UniTask.CompletedTask;
        }
    }
}