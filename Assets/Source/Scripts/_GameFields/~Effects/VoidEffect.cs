using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class VoidEffect : Effect
    {
        private readonly float _delay;

        public VoidEffect(EffectData data) : base(data)
        {
            _delay = 1f;

            Play();
        }

        protected override string GetName() => nameof(VoidEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Пустой эффект закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            await UniTask.WaitForSeconds(_delay, cancellationToken: Token);
        }
    }
}