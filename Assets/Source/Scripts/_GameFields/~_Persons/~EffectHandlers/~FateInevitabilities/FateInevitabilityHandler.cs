using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.SelectMenues;
using Servers;
using Tools;

namespace GameFields.Persons.EffectHandlers.FateInevitabilities
{
    public class FateInevitabilityHandler : EffectHandler, IEffectHandlerActiveWatcher, ILengthyEffectHandler
    {
        private readonly LoseActions _loseActions;
        private readonly ISelectMenuActivator _attackMenu;
        private readonly List<FateInevitabilityHandlerEffect> _activeEffects;

        public FateInevitabilityHandler(LoseActions loseActions, ISelectMenuActivator attackMenu,
            FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            _loseActions = loseActions;
            _attackMenu = attackMenu;

            _activeEffects = new List<FateInevitabilityHandlerEffect>();
        }

        public bool IsActive => _activeEffects.Count > 0;

        public void BeforeEndTurn(CallbackHandler callbackHandler, CancellationToken token)
        {
            Attacking(callbackHandler, token).Forget();
        }

        public void Activate(Card card, int countTurns)
        {
            //if (_activeEffects.Any(e => e.Card == card))
            //    return;

            FateInevitabilityHandlerEffect fateInevitabilityHandlerEffect = new FateInevitabilityHandlerEffect(card, countTurns);
            _activeEffects.Add(fateInevitabilityHandlerEffect);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "ADD", GetType().Name);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _activeEffects.Count.ToString(), "COUNT EFFECTS", GetType().Name);
        }

        public void EndEffect(Card card)
        {
            IEnumerable<FateInevitabilityHandlerEffect> findedEffects = _activeEffects.Where(e => e.Card == card);
            List<FateInevitabilityHandlerEffect> removedEffects = new List<FateInevitabilityHandlerEffect>();

            foreach (FateInevitabilityHandlerEffect findedEffect in findedEffects)
            {
                removedEffects.Add(findedEffect);
            }

            foreach (FateInevitabilityHandlerEffect removedEffect in removedEffects)
            {
                _activeEffects.Remove(removedEffect);
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "REMOVE", GetType().Name);
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _activeEffects.Count.ToString(), "COUNT EFFECTS", GetType().Name);
            }

            //if (_activeEffects.Any(e => e.Card == card))
            //{
            //    findedEffects = _activeEffects.Where(e => e.Card == card);

            //    foreach (FateInevitabilityHandlerEffect findedEffect in findedEffects)
            //    {
            //        _activeEffects.Remove(findedEffect);
            //    }
            //}
        }

        private async UniTask Attacking(CallbackHandler callbackHandler, CancellationToken token)
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

                    await UniTask.WaitUntil(() => _attackMenu.IsComplete, cancellationToken: token);
                }

                if (_activeEffects.Any(e => e.IsReadyToDestroy()))
                {
                    CancellationTokenData cancellationTokenData = new CancellationTokenData(token);
                    _loseActions.Activate(cancellationTokenData);
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