using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers
{
    public class FateInevitabilityHandler: ILengthyEffectHandler
    {
        private readonly LoseActions _loseActions;
        private readonly ISelectMenuActivator _attackMenu;
        private readonly List<FateInevitabilityHandlerEffect> _activeEffects;

        public FateInevitabilityHandler(LoseActions loseActions, ISelectMenuActivator attackMenu)
        {
            _loseActions = loseActions;
            _attackMenu = attackMenu;

            _activeEffects = new List<FateInevitabilityHandlerEffect>();
        }

        public void BeforeEndTurn(CallbackHandler callbackHandler)
        {
            Attacking(callbackHandler).ToUniTask();
        }

        public void Activate(Card card, int countTurns)
        {
            //if (_activeEffects.Any(e => e.Card == card))
            //    return;

            FateInevitabilityHandlerEffect fateInevitabilityHandlerEffect = new FateInevitabilityHandlerEffect(card, countTurns);
            _activeEffects.Add(fateInevitabilityHandlerEffect);
        }

        public void EndEffect(Card card)
        {
            IEnumerable<FateInevitabilityHandlerEffect> findedEffects = _activeEffects.Where(e => e.Card == card);

            if (findedEffects != null)
            {
                foreach (FateInevitabilityHandlerEffect findedEffect in findedEffects)
                {
                    _activeEffects.Remove(findedEffect);
                }
            }
        }

        private IEnumerator Attacking(CallbackHandler callbackHandler)
        {
            if (_activeEffects.Count > 0)
            {
                int countAttack = _activeEffects.Where(e => e.CanActivate()).Count();

                for (int i = 0; i < _activeEffects.Count; i++)
                {
                    _activeEffects[i].Next();
                }

                if (countAttack > 0)
                {
                    SelectMenuActivateData data = new SelectMenuActivateData(countAttack);
                    _attackMenu.Activate(data);

                    yield return new WaitUntil(() => _attackMenu.IsComplete);
                }

                if (_activeEffects.Any(e => e.IsReadyToDestroy()))
                {
                    _loseActions.Activate();
                }
                else
                {
                    callbackHandler.Complete();
                }
            }
            else
            {
                callbackHandler.Complete();
            }
        }
    }
}