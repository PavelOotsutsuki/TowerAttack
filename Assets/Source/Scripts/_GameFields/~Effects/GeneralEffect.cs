using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;

namespace GameFields.Effects
{
    public class GeneralEffect : Effect
    {
        private const int CountNumbers = 4;
        private readonly Person _activePerson;

        public GeneralEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(GeneralEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Генерала закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endChoice = false;

            _activePerson.ChoiceActivate(CountNumbers, () => endChoice = true, RestrictionType.Consecutive);

            await UniTask.WaitUntil(() => endChoice, cancellationToken: Token);
        }
    }
}