using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;

namespace GameFields.Effects
{
    public class CoolBookmakerEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public CoolBookmakerEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(CoolBookmakerEffect);

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