using System.Collections;
using System.Linq;
using GameFields.CardTransits;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class FateMistress_FatefulAttackEffect : Effect
    {
        private readonly Person _activePerson;
        //private readonly Person _deactivePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly LoseActionsRoot _loseActionsRoot;
        private readonly ViewTransitTypesRoot _typesRoot;
        //private readonly SignalBus _bus;

        public FateMistress_FatefulAttackEffect(Person activePerson, LoseActionsRoot loseActionsRoot, CardLocationViewRoot viewRoot,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _loseActionsRoot = loseActionsRoot;
            //_deactivePerson = deactivePerson;
            _viewRoot = viewRoot;
            _typesRoot = typesRoot;
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
            bool isEffectComplete = false;

            //ViewType hand = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            //ViewType hand = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, true);
            ViewType handView = _typesRoot.GetPersonTypes(_activePerson).Hand.ViewType;
            int countCards = _viewRoot.GetAllCards(handView).Count();

            if (countCards == 0)
            {
                CompleteEffect();
                yield break;
            }

            _activePerson.AttackActivate(countCards, CompleteEffect);
            yield return new WaitUntil(() => isEffectComplete); // Спойлер - никогда
        }

        private void CompleteEffect()
        {
            _loseActionsRoot.Capitulate(_activePerson);
            //_isEffectComplete = true;
        }
    }
}