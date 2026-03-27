using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.CardTransits;
using GameFields.Persons;
using GameFields.Persons.EffectHandlers;
using UnityEngine;

namespace GameFields.Effects
{
    public class WiseMonkEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly DiscardManager _discardManager;
        private readonly PersonEffectsHandlerRoot _effectsHandlerRoot;
        private readonly ViewTransitTypesRoot _typesRoot;
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public WiseMonkEffect(Person deactivePerson, CardLocationViewRoot viewRoot,
            DiscardManager discardManager, PersonEffectsHandlerRoot effectsHandlerRoot,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _viewRoot = viewRoot;
            _discardManager = discardManager;
            _effectsHandlerRoot = effectsHandlerRoot;
            _typesRoot = typesRoot;
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

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
            //ViewType table = _deactivePerson is EnemyAI ? ViewType.TableAI : ViewType.TablePlayer;
            //ViewType table = ViewTransitTypeConverter.GetPersonTableViewType(_deactivePerson, true);
            ViewType table = _typesRoot.GetPersonTypes(_deactivePerson).Table;

            IEnumerable<Card> enemyTableCards = _viewRoot.GetAllCards(table);

            //IEnumerable<Card> tableCards = playerTableCards.Union(enemyTableCards);
            IEnumerable<Card> tableCards = enemyTableCards;

            foreach (Card card in tableCards)
            {
                _discardManager.Discard(card);
                _effectsHandlerRoot.EndEffect(card);
            }

            _deactivePerson.ActivateWiseMonkEffect(_card);

            yield break;
        }
    }
}