using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.DiscardPiles;
using GameFields.Persons;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields
{
    public class CardEffects
    {
        private Deck _deck;
        private DiscardPile _discardPile;
        private TableAI _tableAI;
        private TablePlayer _tablePlayer;
        private HandAI _handAI;
        private HandPlayer _handPlayer;
        private TowerAI _towerAI;
        private TowerPlayer _towerPlayer;

        public CardEffects(Deck deck, DiscardPile discardPile)
        {
            _deck = deck;
            _discardPile = discardPile;
        }

        public void PlayEffect(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.Zhyzha_1:
                    PlayEffectZhyzha_1();
                    break;
                case EffectType.Greedy_2:
                    PlayEffectGreedy_2();
                    break;
                case EffectType.PatriarchCorall_26:
                    PlayEffectPatriarchCorall_26();
                    break;
                default:
                    ErrorEffectTypeMessage();
                    break;
            }
        }

        private void PlayEffectZhyzha_1()
        {
            Debug.Log("Эффект Жыжи");
        }

        private void PlayEffectGreedy_2()
        {
            Debug.Log("Эффект Жадины");
        }

        private void PlayEffectPatriarchCorall_26()
        {
            Debug.Log("Эффект Патриарха Коралла");
        }

        private void ErrorEffectTypeMessage()
        {
            Debug.LogError("Unknown EffectType!!!");
        }
    }
}
