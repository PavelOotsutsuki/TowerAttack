using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers
{
    public class FateInevitabilityHandler
    {
        private readonly LoseActions _loseActions;
        private readonly ISelectMenuActivator _attackMenu;
        private readonly List<FateInevitabilityHandlerEffect> _activeEffects;

        //private int _countTurns;
        //private bool _firstTurn;

        public FateInevitabilityHandler(LoseActions loseActions, ISelectMenuActivator attackMenu)
        {
            _loseActions = loseActions;
            _attackMenu = attackMenu;

            _activeEffects = new List<FateInevitabilityHandlerEffect>();
        }

        public void BeforeEndTurn(CallbackHandler callbackHandler)
        {
            //if (_activeEffects.Count > 0)
            //{
            //    List<FateInevitabilityHandlerEffect> activeEffectsClone = new List<FateInevitabilityHandlerEffect>();
            //    activeEffectsClone.AddRange(_activeEffects);

            //    for (int i = 0; i < activeEffectsClone.Count; i++)
            //    {
            //        FateInevitabilityHandlerEffect currentEffect = activeEffectsClone[i];

            //        if (currentEffect.TryActivate() == false)
            //        {
            //            currentEffect.Next();
            //        }
            //        else
            //        {
            //            currentEffect.Next();
            //            Attacking(currentEffect, callbackHandler).ToUniTask();
            //        }
            //    }
            //}
            //else
            //{
            //    callbackHandler.Complete();
            //}
            Attacking(callbackHandler).ToUniTask();
        }

        public void Activate(int countTurns)
        {
            FateInevitabilityHandlerEffect fateInevitabilityHandlerEffect = new FateInevitabilityHandlerEffect(countTurns);
            _activeEffects.Add(fateInevitabilityHandlerEffect);
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




                //List<FateInevitabilityHandlerEffect> activeEffectsClone = new List<FateInevitabilityHandlerEffect>();
                //activeEffectsClone.AddRange(_activeEffects);

                //for (int i = 0; i < activeEffectsClone.Count; i++)
                //{
                //    FateInevitabilityHandlerEffect currentEffect = activeEffectsClone[i];

                //    if (currentEffect.CanActivate() == false)
                //    {
                //        currentEffect.Next();
                //    }
                //    else
                //    {
                //        currentEffect.Next();

                //        SelectMenuActivateData data = new SelectMenuActivateData(1);
                //        _attackMenu.Activate(data);

                //        yield return new WaitUntil(() => _attackMenu.IsComplete);
                //    }
                //}
            }
            else
            {
                callbackHandler.Complete();
            }

            //SelectMenuActivateData data = new SelectMenuActivateData(1);
            //_attackMenu.Activate(data);

            //yield return new WaitUntil(() => _attackMenu.IsComplete);

            //if (currentEffect.IsReadyToDestroy())
            //{
            //    _loseActions.Activate();
            //    _activeEffects.Remove(currentEffect);
            //}
            //else
            //{
            //    callbackHandler.Complete();
            //}
        }

        //public void Actttttivate()
        //{
        //    SelectMenuActivateData data = new SelectMenuActivateData(1);
        //    _attackMenu.Activate(data);

        //}

        //private void ActivateSelectMenu(ISelectMenuActivator selectMenu, int countNumbers, Action callback, RestrictionType? restrictionType)
        //{
        //    SelectMenuActivateData data = new SelectMenuActivateData(countNumbers, restrictionType);

        //    selectMenu.Activate(data);

        //    if (callback != null)
        //        WaitingToInvoke(selectMenu, callback).ToUniTask();
        //}

        //private IEnumerator WaitingToInvoke(ICompletable completable, Action callback)
        //{
        //    yield return new WaitUntil(() => completable.IsComplete);

        //    callback?.Invoke();
        //}
    }
}