using GameFields.Persons.DrawCards;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class RushingMailmanEffect : Effect
    {
        private readonly int _countDrawCards = 2;

        private readonly IDrawCardManager _drawCardManager;

        public RushingMailmanEffect(EffectData data) : base(data)
        {
            _drawCardManager = data.ActivePerson;

            Play();
        }

        protected override string GetName() => nameof(RushingMailmanEffect);

        protected override async UniTask OnPlaying()
        {
            bool isContinue = false;

            _drawCardManager?.DrawCards(_countDrawCards, Token, () => isContinue = true);

            await UniTask.WaitUntil(() => isContinue, cancellationToken: Token);
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("End Несущегося почтальона effect");
        //}
    }
}