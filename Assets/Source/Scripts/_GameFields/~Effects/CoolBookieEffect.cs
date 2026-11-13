using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;

namespace GameFields.Effects
{
    public class CoolBookieEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public CoolBookieEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(CoolBookieEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Четкого букмекера закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endChoice = false;
            _activePerson.ChoiceActivate(CountNumbers, () => endChoice = true, RestrictionType.Even);
            await UniTask.WaitUntil(() => endChoice, cancellationToken: Token);
        }
    }
}