using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Signals;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class FateMistress_FatefulAttackEffect : Effect
    {
        private readonly Person _activePerson;
        //private readonly Person _deactivePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly LoseActionsRoot _loseActionsRoot;
        //private readonly SignalBus _bus;

        private bool _isEffectComplete;

        public FateMistress_FatefulAttackEffect(Person activePerson, LoseActionsRoot loseActionsRoot, CardLocationViewRoot viewRoot,
            EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _loseActionsRoot = loseActionsRoot;
            //_deactivePerson = deactivePerson;
            _viewRoot = viewRoot;
            //_bus = bus;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Судьбоносный удар закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _isEffectComplete = false;

            ViewType hand = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            int countCards = _viewRoot.GetAllCards(hand).Count();

            if (countCards == 0)
            {
                CompleteEffect();
                yield break;
            }

            _activePerson.AttackActivate(countCards, CompleteEffect);
            yield return new WaitUntil(() => _isEffectComplete);
        }

        private void CompleteEffect()
        {
            _loseActionsRoot.Capitulate(_activePerson);
            //_isEffectComplete = true;
        }
    }
}