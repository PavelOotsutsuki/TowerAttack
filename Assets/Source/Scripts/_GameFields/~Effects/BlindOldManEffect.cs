using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;

namespace GameFields.Effects
{
    public class BlindOldManEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public BlindOldManEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(BlindOldManEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект слепого старца закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endPlaying = false;

            _activePerson.ChoiceActivate(CountNumbers, () => endPlaying = true, RestrictionType.Odd);
            await UniTask.WaitUntil(() => endPlaying, cancellationToken: Token);
        }
    }
}