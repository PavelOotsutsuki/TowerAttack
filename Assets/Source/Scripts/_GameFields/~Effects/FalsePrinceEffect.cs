using Cysharp.Threading.Tasks;
using GameFields.Persons;
using Tools;

namespace GameFields.Effects
{
    public class FalsePrinceEffect : Effect
    {
        private readonly Person _activePerson;

        public FalsePrinceEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Лжепринца закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            CallbackHandler callbackHandler = new CallbackHandler();

            _activePerson.ActivateFalsePrinceEffect(callbackHandler);
            await UniTask.WaitUntil(() => callbackHandler.IsComplete, cancellationToken: Token);
        }
    }
}