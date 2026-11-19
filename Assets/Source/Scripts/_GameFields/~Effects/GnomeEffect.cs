using Cysharp.Threading.Tasks;
using GameFields.Persons;

namespace GameFields.Effects
{
    public abstract class GnomeEffect : Effect
    {
        private readonly Person _activePerson;

        public GnomeEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Гнома закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endGmoneSearch = false;
            //_activePerson.ChoiceActivate("Выбрано:", 3);
            if (_activePerson.TryActivateGnomeEffect(out int countNumbers))
            {
                _activePerson.ChoiceActivate(countNumbers, () => endGmoneSearch = true);
                await UniTask.WaitUntil(() => endGmoneSearch, cancellationToken: Token);
            }

            //_activePerson.ChoiceActivate(4, EndPlayingCallback, RestrictionType.Consecutive);
            //yield return new WaitUntil(() => _activePerson.IsChoiceComplete);

            //_deactivePerson.AttackDeactivate();
        }
    }
}