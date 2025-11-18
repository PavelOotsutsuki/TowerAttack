using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EffectHandlers.Fires;
using GameFields.Persons.Tables;
using Tools.Utils;
using UnityEngine;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class HardAIThinkLogic
    {
        private readonly ICardWatcher _cardWatcher;
        private readonly ICardCheck _deck;
        private readonly ConfirmableNumbers _confirmableNumbers;
        private readonly GnomeEffectHandler _gnomeEffectHandler;
        private readonly ICardDropPlace _table;
        private readonly ICardView _handEnemy;
        private readonly IFireEffectHandler _fireEffectHandler;
        private readonly ICardView _discardPile;
        private readonly ICardView _fireRoot;

        public HardAIThinkLogic(ICardWatcher cardWatcher, ICardCheck deck, ConfirmableNumbers confirmableNumbers,
            GnomeEffectHandler gnomeEffectHandler, ICardDropPlace table, ICardView handEnemy, IFireEffectHandler fireEffectHandler,
            ICardView discardPile, ICardView fireRoot)
        {
            _cardWatcher = cardWatcher;
            _deck = deck;
            _confirmableNumbers = confirmableNumbers;
            _gnomeEffectHandler = gnomeEffectHandler;
            _table = table;
            _handEnemy = handEnemy;
            _fireEffectHandler = fireEffectHandler;
            _discardPile = discardPile;
            _fireRoot = fireRoot;
        }

        private int MaxGnomeCards => _cardWatcher.Cards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count();
        private int PlayedGnomeCards => _discardPile.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count()
            + _fireRoot.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count();

        public CardCapability FindActionType(CardCapability cardCapability)
        {
            List<CardCapability> testflags = Utils.GetFlags(cardCapability & (CardCapability.Play |
                CardCapability.GnomeForging | CardCapability.HandTransfer));

            if (testflags.Count == 0)
                return CardCapability.Attack;

            return testflags[Random.Range(0, testflags.Count)];

            List<CardCapability> flags = Utils.GetFlags(cardCapability & (CardCapability.Attack | CardCapability.Play |
                CardCapability.GnomeForging | CardCapability.HandTransfer));

            //if ((cardCapability & CardCapability.GnomeForging) == CardCapability.GnomeForging)
            //{
            //    if ((cardCapability & CardCapability.Attack) == CardCapability.Attack)
            //    {

            //    }

            //    if ((cardCapability & CardCapability.Play) == CardCapability.Play)
            //    {
            //        if ()
            //    }
            //}
            if (flags.Count <= 0)
                throw new System.Exception("Ошибка задания флага. Вернулся пустой список");

            if (flags.Contains(CardCapability.Play))
                if (_table.HasFreeSeat == false)
                    flags.Remove(CardCapability.Play);

            if (flags.Count == 1)
                return flags[0];

            Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();
            // Пытаемся пойти нетривиальным путем (не Attack и не Play)
            // HandTransfer - не реализовано тк пока он всегда подходит под условие flags.Count == 1
            // GnomeForging - пока единственное нетривиальное условие которое нужно описать
            if (flags.Contains(CardCapability.GnomeForging))
            {
                //Если GnomeChoice уже был разыгран
                if (_gnomeEffectHandler.CanActivate() == false)
                    if ((cardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice)
                    {
                        probability.Add(CardCapability.Play, 5); // Берем минимальную вероятность, но все же берем

                        int gnomeForgingProbability = 7; // По дефолту будет 7. Если мы разыграли GnomeChoice, то эффективность
                                                         // зависит только от того нужно ли брать карту

                        if (_fireEffectHandler.IsFireMode) 
                            gnomeForgingProbability -= 5; 

                        if (_deck.IsHasCards(1))
                            gnomeForgingProbability += 5;

                        // Если взять карту, эффективность хода увеличится, но если она сожжется то эффект нивилируется.
                        // Аналогично, если сжечь карту эффективность уменьшится, если карт нет то и норм
                        probability.Add(CardCapability.GnomeForging, gnomeForgingProbability);
                        // Остается атака, самое вероятное
                        probability.Add(CardCapability.Attack, 80);

                        return CalculateByProbability(probability);
                    }

                // Если апгрейд достиг того количества, которое мало влияет на исход угадывания, апгредом заниматься больше не надо
                if (_gnomeEffectHandler.GnomeCounterNumbers * 2 + _gnomeEffectHandler.UpgradeStepCount * 2 > _confirmableNumbers.FreeNumbers.Count())
                {
                    int gnomeForgingProbability = 7; // По дефолту будет 7. Если мы разыграли не хотим ковать по кол-ву угаданных
                                                     // номеров, то эффективность зависит только от того нужно ли брать карту

                    if (_fireEffectHandler.IsFireMode)
                        gnomeForgingProbability -= 5;

                    if (_deck.IsHasCards(1))
                        gnomeForgingProbability += 5;

                    // Если взять карту, эффективность хода увеличится, но если она сожжется то эффект нивилируется.
                    // Аналогично, если сжечь карту эффективность уменьшится, если карт нет то и норм
                    probability.Add(CardCapability.GnomeForging, gnomeForgingProbability);
                    //Самое время разыграть 
                    probability.Add(CardCapability.Play, 80);
                    // Атаковать в данной ситуации очень плохо
                    probability.Add(CardCapability.Attack, 5);

                    return CalculateByProbability(probability);
                }

                //Если гномих карт осталось 1 или меньше, берем максимальную вероятность
                if ((MaxGnomeCards - PlayedGnomeCards - 1) < 2) // (MaxGnomeCards - 1) потому что не учитываем в расчетах эту карту
                {
                    probability.Add(CardCapability.Play, 80);

                    int gnomeForgingProbability = 7; // По дефолту будет 7. Если мы очень хотим разыграть,
                                                     // то эффективность ковки зависит только от того нужно ли брать карту

                    if (_fireEffectHandler.IsFireMode)
                        gnomeForgingProbability -= 5;

                    if (_deck.IsHasCards(1))
                        gnomeForgingProbability += 5;

                    probability.Add(CardCapability.GnomeForging, gnomeForgingProbability);

                    probability.Add(CardCapability.Attack, 5);
                    return CalculateByProbability(probability);
                }

                if ((MaxGnomeCards - PlayedGnomeCards - 1) < 3) // Вероятность меньше, но все равно сыграть хочется
                {
                    probability.Add(CardCapability.Play, 60);

                    int gnomeForgingProbability = 15; // По дефолту будет 15. Если мы очень хотим разыграть,
                                                     // то эффективность ковки зависит только от того нужно ли брать карту

                    if (_fireEffectHandler.IsFireMode)
                        gnomeForgingProbability -= 5;

                    if (_deck.IsHasCards(1))
                        gnomeForgingProbability += 5;

                    probability.Add(CardCapability.GnomeForging, gnomeForgingProbability);

                    probability.Add(CardCapability.Attack, 5);
                    return CalculateByProbability(probability);
                }

                if ((MaxGnomeCards - PlayedGnomeCards - 1) < 4) // Вероятность меньше, но все равно сыграть хочется
                {
                    probability.Add(CardCapability.Play, 40);

                    int gnomeForgingProbability = 35; // По дефолту будет 35. Если мы очень хотим разыграть,
                                                      // то эффективность ковки зависит только от того нужно ли брать карту

                    if (_fireEffectHandler.IsFireMode)
                        gnomeForgingProbability -= 5;

                    if (_deck.IsHasCards(1))
                        gnomeForgingProbability += 5;

                    probability.Add(CardCapability.GnomeForging, gnomeForgingProbability);

                    probability.Add(CardCapability.Attack, 5);
                    return CalculateByProbability(probability);
                }

                // В остальных случаях отдаем предпочтение ковке без разбирательств
                probability.Add(CardCapability.GnomeForging, 80);
                probability.Add(CardCapability.Play, 10);
                probability.Add(CardCapability.Attack, 5);
                return CalculateByProbability(probability);
            }

            // Тривиальный путь. Что лучше, разыграть карту или атаковать?
            // По большей части логика одна - чем больше неугаданных номеров, тем выше вероятность Play
            // Есть нюансы когда эффекты карты ссылают на то, чего нет, и поэтому ничего не делают, но это потом реализовать
            int countAll = _cardWatcher.Cards.Count;
            int countFree = _confirmableNumbers.FreeNumbers.Count();

            //Debug.Log($"{countAll}: countAll");
            //Debug.Log($"{countFree}: countFree");

            int probabilityPlay = countFree * 100 / countAll;

            if (probabilityPlay > 99)
                probabilityPlay = 99;

            int probabilityAttack = 100 - probabilityPlay;

            probability.Add(CardCapability.Play, probabilityPlay);
            probability.Add(CardCapability.Attack, probabilityAttack);
            return CalculateByProbability(probability);
        }

        private CardCapability CalculateByProbability(IReadOnlyDictionary<CardCapability, int> probability)
        {
            int maxValue = probability.Select(p => p.Value).Sum();

            int randomValue = Random.Range(0, maxValue);
            int summ = 0;

            #region Debug
            string debugMsg = "";
            foreach (KeyValuePair<CardCapability, int> pair in probability)
            {
                summ += pair.Value;
                debugMsg += $"{pair.Key}: value: {pair.Value} summ: {summ} randomValue: {randomValue}\n";
            }
            Debug.Log(debugMsg);
            summ = 0;
            #endregion

            foreach (KeyValuePair<CardCapability, int> pair in probability)
            {
                if (randomValue < pair.Value + summ)
                {
                    return pair.Key;
                }

                summ += pair.Value;
            }

            Debug.Log("Сюда никогда не дойдет");
            return probability.Last().Key;
        }
    }
}