using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class BrothersMotherEffect : Effect
    {
        private const int UpgradeCount = 2;

        private readonly Person _activePerson;

        public BrothersMotherEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(BrothersMotherEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Матери братьев закончен");
        //}

        protected override UniTask OnPlaying()
        {
            _activePerson.UpgradeBrothers(UpgradeCount);
            return UniTask.CompletedTask;
        }
    }
}