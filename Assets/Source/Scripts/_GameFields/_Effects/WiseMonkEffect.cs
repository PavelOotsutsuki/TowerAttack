using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers;
using UnityEngine;

namespace GameFields.Effects
{
    public class WiseMonkEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly DiscardManager _discardManager;
        private readonly PersonEffectsHandlerRoot _effectsHandlerRoot;
        private readonly Person _deactivePerson;
        private readonly Person _activePerson;

        public WiseMonkEffect(Person deactivePerson, Person activePerson, CardLocationViewRoot viewRoot,
            DiscardManager discardManager, PersonEffectsHandlerRoot effectsHandlerRoot, EffectData data) : base(data)
        {
            _viewRoot = viewRoot;
            _discardManager = discardManager;
            _effectsHandlerRoot = effectsHandlerRoot;
            _deactivePerson = deactivePerson;
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Мудрого монаха закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //IEnumerable<Card> playerTableCards = _viewRoot.GetAllCards(ViewType.TablePlayer);
            ViewType table = _deactivePerson is EnemyAI ? ViewType.TableAI : ViewType.TablePlayer;
            
            IEnumerable<Card> enemyTableCards = _viewRoot.GetAllCards(table);

            //IEnumerable<Card> tableCards = playerTableCards.Union(enemyTableCards);
            IEnumerable<Card> tableCards = enemyTableCards;

            foreach (Card card in tableCards)
            {
                _discardManager.Discard(card);
                _effectsHandlerRoot.EndEffect(card);
            }

            yield break;
        }
    }
}