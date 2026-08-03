using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class ThreeGuysEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public ThreeGuysEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(ThreeGuysEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Трех Бугаев закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endChoice = false;

            _activePerson.ChoiceImitationActivate(CountNumbers, () => endChoice = true);

            await UniTask.WaitUntil(() => endChoice, cancellationToken: Token);
        }
    }
}