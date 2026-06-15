using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class CowsHerdEffect : Effect
    {
        private const int DefaultValueAttack = 2;
        private const int BonusByEffect = 1;

        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;

        public CowsHerdEffect(CardLocationViewRoot viewRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _viewRoot = viewRoot;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Стадо коров закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endPlayingAttack = false;

            int countAttack = DefaultValueAttack;
            int countCardsInTable = _viewRoot.GetAllCards(ViewType.TableAI).Count() + _viewRoot.GetAllCards(ViewType.TablePlayer).Count();

            if (countCardsInTable > 0)
                countAttack += BonusByEffect;

            _activePerson.AttackActivate(countAttack, () => endPlayingAttack = true);
            await UniTask.WaitUntil(() => endPlayingAttack, cancellationToken: Token);
        }
    }
}