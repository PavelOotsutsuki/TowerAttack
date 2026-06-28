using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.DependencyInterlayers;
using Cards.Effects;
using Cards.Views;
using GameFields.CardTransits;
using GameFields.Persons;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EffectHandlers.Fires;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.Tables;
using Tools.Settings;
using Tools.Utils;
using UnityEngine;
using Random = UnityEngine.Random;
using GameFields.Persons.EffectHandlers.FateInevitabilities;
using GameFields.Persons.EffectHandlers.Scarecrows;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class VeryHardAIThinkLogic : IAIThinkLogic
    {
        private readonly ICardWatcher _cardWatcher;
        private readonly ICardView _deck;
        private readonly ConfirmableNumbers _confirmableNumbersEnemy;
        private readonly ConfirmableNumbers _confirmableNumbersPlayer;
        private readonly GnomeEffectHandler _gnomeEffectHandler;
        private readonly ICardDropPlace _cardPlayingZone;
        private readonly ICardView _handEnemy;
        private readonly IEffectHandlerActiveWatcher _fireEffectHandlerEnemy;
        private readonly IEffectHandlerActiveWatcher _fireEffectHandlerPlayer;
        private readonly ICardView _discardPile;
        private readonly ICardView _fireRoot;
        private readonly ICardView _handPlayer;
        private readonly IReadOnlyPersonEffectKeeper _playerPersonEffectKeeper;
        private readonly BrothersEffectHandler _brothersEffectHandlerEnemy;
        private readonly ICardView _tableEnemy;
        private readonly ICardView _tablePlayer;
        private readonly IEffectHandlerActiveWatcher _skipTurnEffectHandlerPlayer;
        private readonly IEffectHandlerActiveWatcher _fateInevitabilityHandlerPlayer;
        private readonly IEffectHandlerActiveWatcher _wiseMonkEffectHandlerEnemy;
        private readonly IEffectHandlerActiveWatcher _scarecrowEffectHandlerEnemy;

        private bool _isScarecrowMode = false; // Если чучело разыграно, мы наоборот хотим разыграть карту которая наименее полезна. И мы всегда разыгрываем карту!
        private Card _workCard;

        public VeryHardAIThinkLogic(ICardWatcher cardWatcher, ICardView deck, ConfirmableNumbers confirmableNumbersEnemy,
            GnomeEffectHandler gnomeEffectHandler, ICardDropPlace cardPlayingZone, ICardView handEnemy, FireEffectHandler fireEffectHandlerEnemy,
            ICardView discardPile, ICardView fireRoot, ICardView handPlayer, IReadOnlyPersonEffectKeeper playerPersonEffectKeeper,
            ConfirmableNumbers confirmableNumbersPlayer, BrothersEffectHandler brothersEffectHandlerEnemy, Table tableEnemy,
            Table tablePlayer, SkipTurnEffectHandler skipTurnEffectHandlerPlayer, FateInevitabilityHandler fateInevitabilityHandlerPlayer,
            FireEffectHandler fireEffectHandlerPlayer, WiseMonkEffectHandler wiseMonkEffectHandlerEnemy, ScarecrowEffectHandler scarecrowEffectHandlerEnemy)
        {
            _cardWatcher = cardWatcher;
            _deck = deck;
            _confirmableNumbersEnemy = confirmableNumbersEnemy;
            _gnomeEffectHandler = gnomeEffectHandler;
            _cardPlayingZone = cardPlayingZone;
            _handEnemy = handEnemy;
            _fireEffectHandlerEnemy = fireEffectHandlerEnemy;
            _fireEffectHandlerPlayer = fireEffectHandlerPlayer;
            _discardPile = discardPile;
            _fireRoot = fireRoot;
            _handPlayer = handPlayer;
            _playerPersonEffectKeeper = playerPersonEffectKeeper;
            _confirmableNumbersPlayer = confirmableNumbersPlayer;
            _brothersEffectHandlerEnemy = brothersEffectHandlerEnemy;
            _tableEnemy = tableEnemy;
            _tablePlayer = tablePlayer;
            _skipTurnEffectHandlerPlayer = skipTurnEffectHandlerPlayer;
            _fateInevitabilityHandlerPlayer = fateInevitabilityHandlerPlayer;
            _wiseMonkEffectHandlerEnemy = wiseMonkEffectHandlerEnemy;
            _scarecrowEffectHandlerEnemy = scarecrowEffectHandlerEnemy;
        }

        private int MaxGnomeCards => _cardWatcher.Cards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count();
        private int PlayedGnomeCards => _discardPile.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count()
                + _fireRoot.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count();
        private bool IsHasExtraGnomeInHand => _handEnemy.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count() > (_workCard.CardName == CardName.TimeLord ? 0 : 1);

        private bool IsDoubleEffect => _tableEnemy.AllCards.Select(c => c.CardName).Contains(CardName.Schemer) || _tablePlayer.AllCards.Select(c => c.CardName).Contains(CardName.Schemer);
        private int DoubleEffectFactor => IsDoubleEffect ? 2 : 1;
        //private int _power;
        //public int Power
        //{
        //    get
        //    {
        //        int tempPower = _power;
        //        _power = 0;
        //        return tempPower;
        //    }
        //    private set
        //    {
        //        _power = value;
        //    }
        //}
        private IReadOnlyList<CardCapability> _currentFlags;


        public CapabilityProbability FindActionType(Card workCard)
        {
            _isScarecrowMode = false;
            _workCard = workCard;

            CardCapability cardCapability = workCard.CardCapability;

            //List<CardCapability> testflags = Utils.GetFlags(cardCapability & (CardCapability.Play |
            //    CardCapability.GnomeForging | CardCapability.HandTransfer));

            //if (testflags.Count == 0)
            //    return CardCapability.Attack;

            //return testflags[Random.Range(0, testflags.Count)];

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

            // Если 1 номер остался, то атакуем конечно же
            if (flags.Contains(CardCapability.Attack))
                if (_confirmableNumbersEnemy.FreeNumbers.Count() <= 1)
                    return new CapabilityProbability(CardCapability.Attack, 100);

            // Если места на столе нет, или противником разыгран Мудрый Монах (32), карту точно не играем
            if (flags.Contains(CardCapability.Play))
                if (_cardPlayingZone.HasFreeSeat == false || _wiseMonkEffectHandlerEnemy.IsActive)
                    flags.Remove(CardCapability.Play);

            //if (flags.Count == 1)
            //    return new CapabilityProbability(flags[0], 90);

            // Если противник разыграл Чучело (30), мы хотим его затриггерить как можно скорее (логика опциональна)
            if (flags.Contains(CardCapability.Play) && _scarecrowEffectHandlerEnemy.IsActive)
                _isScarecrowMode = true;

            _currentFlags = flags;

            CapabilityProbability capabilityProbability = FindCapabilityByEffect(workCard.EffectConfig.Type, flags);

            if (_isScarecrowMode == true && capabilityProbability.CardCapability == CardCapability.Play && workCard.CardName != CardName.TimeLord)
            {
                int newProbability = 1000 - capabilityProbability.Probability;

                capabilityProbability = new CapabilityProbability(capabilityProbability.CardCapability, newProbability);
            }

            if (workCard.IsCurse)
                capabilityProbability = new CapabilityProbability(capabilityProbability.CardCapability, (capabilityProbability.Probability + 50) * 2);

            return capabilityProbability;
            //if (_isScarecrowMode)
        }

        private CapabilityProbability FindCapabilityByEffect(EffectType effectType, IReadOnlyList<CardCapability> flags)
        {
            switch (effectType)
            {
                case EffectType.Void:
                    return new CapabilityProbability(CardCapability.Attack, 40);
                case EffectType.Zhyzha:
                    return GetZhyzhaCapability();
                case EffectType.Greedy:
                    return GetGreedyCapability();
                case EffectType.Pyromancer:
                    return GetPyromancerCapability();
                case EffectType.CoolBookmaker:
                    return GetCoolBookmakerCapability();
                case EffectType.BlindOldMan:
                    return GetBlindOldManCapability();
                case EffectType.DetectiveRhodes:
                    return GetDetectiveRhodesCapability();
                case EffectType.BlueGnome:
                case EffectType.RedGnome:
                case EffectType.GreenGnome:
                case EffectType.WhiteGnome:
                case EffectType.BlackGnome:
                    return GetGnomeCapability(flags);
                case EffectType.TimeLord:
                    return GetTimeLordCapability();
                case EffectType.ThreeGuys:
                    return GetThreeGuysCapability();
                case EffectType.TimeMistress:
                    return GetTimeMistressCapability();
                case EffectType.SharpSnake:
                    return GetSharpSnakeCapability();
                case EffectType.ImpArmy:
                    return GetImpArmyCapability();
                case EffectType.CursedMark:
                    return GetCursedMarkCapability();
                case EffectType.RushingMailman:
                    return GetRushingMailmanCapability();
                case EffectType.Schemer:
                    return GetSchemerCapability();
                case EffectType.Mime:
                    return GetMimeCapability();
                case EffectType.TimeChild:
                    return GetTimeChildCapability();
                case EffectType.Undergrounder:
                    return GetUndergrounderCapability();
                case EffectType.RobinGood:
                    return GetRobinGoodCapability();
                case EffectType.General:
                    return GetGeneralCapability();
                case EffectType.FateMistress:
                    return GetFateMistressCapability();
                case EffectType.DumbMonk:
                    return GetDumbMonkCapability();
                case EffectType.LeftEyedSister:
                    return GetLeftEyedSisterCapability();
                case EffectType.JusticeBull:
                    return GetJusticeBullCapability();
                case EffectType.PatriarchCorall:
                    return GetPatriarchCorallCapability();
                case EffectType.LittleBrother:
                    return GetLittleBrotherCapability();
                case EffectType.BrothersMother:
                    return GetBrothersMotherCapability();
                case EffectType.Scarecrow:
                    return GetScarecrowCapability();
                case EffectType.LuckyHorseshoe:
                    return GetLuckyHorseshoeCapability();
                case EffectType.WiseMonk:
                    return GetWiseMonkCapability();
                case EffectType.CowsHerd:
                    return GetCowsHerdCapability();
                case EffectType.HungryOgre:
                    return GetHungryOgreCapability();
                case EffectType.Sharper:
                    return GetSharperCapability();
                case EffectType.Gunner:
                    return GetGunnerCapability();
                case EffectType.MiddleBrother:
                    return GetMiddleBrotherCapability();
                case EffectType.DeadOgre:
                    return GetDeadOgreCapability();
                case EffectType.OutOfControlBus:
                    return GetOutOfControlBusCapability();
                case EffectType.CursedMailman:
                    return GetCursedMailmanCapability();
                case EffectType.RightEyedSister:
                    return GetRightEyedSisterCapability();
                case EffectType.StrongOgre:
                    return GetStrongOgreCapability();
                case EffectType.MafiaBoss:
                    return GetMafiaBossCapability();
                case EffectType.PyromancersManuscript:
                    return GetPyromancersManuscriptCapability();
                case EffectType.FalsePrince:
                    return GetFalsePrinceCapability();
                case EffectType.BigBrother:
                    return GetBigBrotherCapability();
                case EffectType.LastChance:
                    return GetLastChanceCapability();
                case EffectType.FallenGuardian:
                    return GetFallenGuardianCapability();
                default:
                    throw new Exception($"Неизвестный эффект: {effectType}");
            }

            //return GetDefaultCapability(30);
        }

        private CapabilityProbability CalculateByProbability(IReadOnlyDictionary<CardCapability, int> probability)
        {
            int maxValue = probability.Select(p => p.Value).Sum();

            int randomValue = Random.Range(1, maxValue);
            int summ = 0;

            //#region Debug
            //string debugMsg = "";
            //foreach (KeyValuePair<CardCapability, int> pair in probability)
            //{
            //    summ += pair.Value;
            //    debugMsg += $"{pair.Key}: value: {pair.Value} summ: {summ} randomValue: {randomValue}\n";
            //}
            //Debug.Log(debugMsg);
            //summ = 0;
            //#endregion

            foreach (KeyValuePair<CardCapability, int> pair in probability)
            {
                if (randomValue < pair.Value + summ)
                {
                    int probabilityValue = maxValue + randomValue / 2;

                    if (pair.Key == CardCapability.Attack)
                    {
                        probabilityValue = pair.Value;
                    }

                    return new CapabilityProbability(pair.Key, probabilityValue);
                }

                summ += pair.Value;
            }

            Debug.Log("Сюда никогда не дойдет");
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
            return new CapabilityProbability(probability.Last().Key, 20);
        }

        /// <summary>
        /// minPlayChance - Каков шанс розыгрыша карты когда номеров осталось угадывать не так уж и много
        /// </summary>
        /// <param name="minPlayChance">Каков шанс розыгрыша карты когда номеров осталось угадывать не так уж и много</param>
        /// <returns></returns>
        private CapabilityProbability GetDefaultCapability(int minPlayChance)
        {
            if (minPlayChance > 100)
                minPlayChance = 99;

            //if (minPlayChance < 0)
            //    minPlayChance = 0;

            Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();
            // Тривиальный путь. Что лучше, разыграть карту или атаковать?
            // По большей части логика одна - чем больше неугаданных номеров, тем выше вероятность Play
            // Есть нюансы когда эффекты карты ссылают на то, чего нет, и поэтому ничего не делают, но это потом реализовать
            int countAll = _cardWatcher.Cards.Count;
            int countFree = _confirmableNumbersEnemy.FreeNumbers.Count();

            //Debug.Log($"{countAll}: countAll");
            //Debug.Log($"{countFree}: countFree");

            //int probabilityPlay = countFree * 100 / countAll;
            int probabilityPlay = minPlayChance * 2 + countFree * (100 - minPlayChance) / countAll;

            if (probabilityPlay < 0)
                probabilityPlay = 0;

            // Если включены двойные эффекты, эффективность розыгрыша уделичивается на minPlayChance. Для некоторых карт конечно ничего не меняется, но впадлу писать скрипт для всех отдельно
            //if (_tableEnemy.AllCards.Select(c => c.CardName).Contains(CardName.Schemer) || _tablePlayer.AllCards.Select(c => c.CardName).Contains(CardName.Schemer))
            //    probabilityPlay += minPlayChance;

                //if (probabilityPlay > 99)
                //    probabilityPlay = 99;

            int probabilityAttack = 100 - probabilityPlay;

            if (probabilityAttack < 1)
                probabilityAttack = 1;


            AddProbability(probability, CardCapability.Play, probabilityPlay);
            AddProbability(probability, CardCapability.Attack, probabilityAttack);

            // Если ничего не могу сделать, верну null
            if (probability.Count == 0)
                return null;

            return CalculateByProbability(probability);
        }

        // 7, 17, 27, 37, 47
        private CapabilityProbability GetGnomeCapability(IReadOnlyList<CardCapability> flags)
        {
            //if (flags.Contains(CardCapability.GnomeForging) == false || flags.Contains(CardCapability.GnomeChoice) == false)
            //    throw new Exception("Попытка ковки нековающей карты");

            #region Debug
            string debugMsg = "";

            foreach (CardCapability cardCapability in flags)
            {
                if (debugMsg != "")
                    debugMsg += ", ";

                debugMsg += cardCapability.ToString();
            }
            //Debug.Log(debugMsg);
            #endregion

            Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();

            //Если GnomeChoice уже был разыгран или ещё гном есть в руке
            if (_gnomeEffectHandler.CanActivate() == false ||
                IsHasExtraGnomeInHand)
            {
                AddProbability(probability, CardCapability.Play, 5); // Берем минимальную вероятность, но все же берем

                int gnomeForgingProbability = _deck.AllCards.Count() * 90 / GameSettings.DefaultCardNumbers.Length; // По дефолту будет 7. Если мы разыграли GnomeChoice, то эффективность
                                                 // зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
                    gnomeForgingProbability -= 5;

                if (_deck.IsHasCards(1))
                    gnomeForgingProbability += 5;

                // Если взять карту, эффективность хода увеличится, но если она сожжется то эффект нивилируется.
                // Аналогично, если сжечь карту эффективность уменьшится, если карт нет то и норм
                AddProbability(probability, CardCapability.GnomeForging, gnomeForgingProbability);
                // Остается атака, самое вероятное
                AddProbability(probability, CardCapability.Attack, 90 - gnomeForgingProbability);

                return CalculateByProbability(probability);
            }

            if (_deck.IsHasCards(1) == false && IsHasExtraGnomeInHand == false)
            {
                AddProbability(probability, CardCapability.Play, 90);

                int gnomeForgingProbability = 5; // Смысл 0, но на рандом оставлю

                AddProbability(probability, CardCapability.GnomeForging, gnomeForgingProbability);
                AddProbability(probability, CardCapability.Attack, 5);

                return CalculateByProbability(probability);
            }

            // Если апгрейд достиг того количества, которое мало влияет на исход угадывания, апгредом заниматься больше не надо
            if (_gnomeEffectHandler.GnomeCounterNumbers * 2 + _gnomeEffectHandler.UpgradeStepCount * 2 > _confirmableNumbersEnemy.FreeNumbers.Count())
            {
                int gnomeForgingProbability = 7; // По дефолту будет 7. Если мы разыграли не хотим ковать по кол-ву угаданных
                                                    // номеров, то эффективность зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
                    gnomeForgingProbability -= 5;

                if (_deck.IsHasCards(1))
                    gnomeForgingProbability += 5;

                // Если взять карту, эффективность хода увеличится, но если она сожжется то эффект нивилируется.
                // Аналогично, если сжечь карту эффективность уменьшится, если карт нет то и норм
                AddProbability(probability, CardCapability.GnomeForging, gnomeForgingProbability);
                //Самое время разыграть
                AddProbability(probability, CardCapability.Play, 80);
                // Атаковать в данной ситуации очень плохо
                AddProbability(probability, CardCapability.Attack, 5);

                return CalculateByProbability(probability);
            }

            //Если гномих карт осталось 1 или меньше, берем максимальную вероятность
            if ((MaxGnomeCards - PlayedGnomeCards - 1) < 2) // (MaxGnomeCards - 1) потому что не учитываем в расчетах эту карту
            {
                AddProbability(probability, CardCapability.Play, 80);

                int gnomeForgingProbability = 15; // По дефолту будет 7. Если мы очень хотим разыграть,
                                                    // то эффективность ковки зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
                    gnomeForgingProbability -= 5;

                if (_deck.IsHasCards(1))
                    gnomeForgingProbability += 5;

                AddProbability(probability, CardCapability.GnomeForging, gnomeForgingProbability);
                AddProbability(probability, CardCapability.Attack, 5);

                return CalculateByProbability(probability);
            }

            if ((MaxGnomeCards - PlayedGnomeCards - 1) < 3) // Вероятность меньше, но все равно сыграть хочется
            {
                AddProbability(probability, CardCapability.Play, 50);

                int gnomeForgingProbability = 45; // По дефолту будет 15. Если мы очень хотим разыграть,
                                                    // то эффективность ковки зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
                    gnomeForgingProbability -= 5;

                if (_deck.IsHasCards(1))
                    gnomeForgingProbability += 5;

                AddProbability(probability, CardCapability.GnomeForging, gnomeForgingProbability);
                AddProbability(probability, CardCapability.Attack, 5);

                return CalculateByProbability(probability);
            }

            if ((MaxGnomeCards - PlayedGnomeCards - 1) < 4) // Вероятность меньше, но все равно сыграть хочется
            {
                AddProbability(probability, CardCapability.Play, 10);

                int gnomeForgingProbability = 85; // По дефолту будет 35. Если мы очень хотим разыграть,
                                                    // то эффективность ковки зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
                    gnomeForgingProbability -= 5;

                if (_deck.IsHasCards(1))
                    gnomeForgingProbability += 5;

                AddProbability(probability, CardCapability.GnomeForging, gnomeForgingProbability);
                AddProbability(probability, CardCapability.Attack, 5);

                return CalculateByProbability(probability);
            }

            // В остальных случаях отдаем предпочтение ковке без разбирательств
            AddProbability(probability, CardCapability.GnomeForging, 80);
            AddProbability(probability, CardCapability.Play, 10);
            AddProbability(probability, CardCapability.Attack, 5);

            return CalculateByProbability(probability);
        }

        private void AddProbability(Dictionary<CardCapability, int> probabilities, CardCapability capability, int probability)
        {
            if (_currentFlags.Contains(capability))
                probabilities.Add(capability, probability);
        }

        // 1
        private CapabilityProbability GetZhyzhaCapability()
        {
            // Если карт нет, противник пропустит 2 хода, что весьма выгодно
            if (_deck.IsHasCards(1) == false)
                //|| _fireEffectHandler.IsFireMode) // Если fire убрал, тк не могу проверить уйдет ли сейчас пиромант в бито или нет. Да и толку мало
            {
                return GetDefaultCapability(80);
            }

            // В остальных случаях обычная логика.
            return GetDefaultCapability(15);
        }

        // 2
        private CapabilityProbability GetGreedyCapability()
        {
            if (IsDoubleEffect)
                return new CapabilityProbability(CardCapability.Attack, 10);
            // Пока все просто. Если у противника (player) больше карт чем у меня (enemy), значит я хочу обменяться. Иначе, атаковать.
            int enemyHandCardsCount = _handEnemy.AllCards.Count();
            int playerHandCardsCount = _handPlayer.AllCards.Count();

            return GetDefaultCapability((playerHandCardsCount - 1 - enemyHandCardsCount) * 5);

            //if (enemyHandCardsCount < playerHandCardsCount - 1)
            //{
            //    return GetDefaultCapability(35);
            //}

            //// В остальных случаях обычная логика.
            //return GetDefaultCapability(0);
        }

        // 3
        private CapabilityProbability GetPyromancerCapability()
        {
            //Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();

            // Если карты есть, то есть что сжигать. Значит хорошо.
            // Есть мин 5 карты, шанс наибольший что 2 хода сработает пиромант
            if (_deck.IsHasCards(5))
            //|| _fireEffectHandler.IsFireMode) // Если fire убрал, тк не могу проверить уйдет ли сейчас пиромант в бито или нет. Да и толку мало
            {
                return GetDefaultCapability(20);
            }

            // Пиромант сработает 1 ход, толк меньше но есть
            if (_deck.IsHasCards(1))
            //|| _fireEffectHandler.IsFireMode) // Если fire убрал, тк не могу проверить уйдет ли сейчас пиромант в бито или нет. Да и толку мало
            {
                return GetDefaultCapability(0);
            }

            // В остальных случаях атакуем
            return new CapabilityProbability(CardCapability.Attack, 60);
        }

        // 4 
        private CapabilityProbability GetCoolBookmakerCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 5 && freeNumbers.Where(n => n % 2 == 0).Count() >= 3)
            {
                return GetDefaultCapability(35 * DoubleEffectFactor);
            }

            // В остальных случаях атакуем
            return new CapabilityProbability(CardCapability.Attack, 40);
        }

        // 5
        private CapabilityProbability GetBlindOldManCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 5 && freeNumbers.Where(n => n % 2 == 1).Count() >= 3)
            {
                return GetDefaultCapability(35 * DoubleEffectFactor);
            }

            // В остальных случаях атакуем
            return new CapabilityProbability(CardCapability.Attack, 40);
        }

        // 6
        private CapabilityProbability GetDetectiveRhodesCapability()
        {
            // Если нет карт ни в деке, ни в руке противника играть бессмысленно
            if (_handPlayer.AllCards.Count() == 0 || _deck.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 80);

            // В остальных случаях базовая логика
            return GetDefaultCapability(20 * DoubleEffectFactor);
        }

        // 8
        private CapabilityProbability GetTimeLordCapability()
        {
            if (_playerPersonEffectKeeper.PersonEffect == null)
                return new CapabilityProbability(CardCapability.Attack, 10);

            if (_playerPersonEffectKeeper.PersonEffect.CardEffectConfig.Type == EffectType.TimeLord)
                return new CapabilityProbability(CardCapability.Attack, 10);

            return FindActionType(_playerPersonEffectKeeper.PersonEffect.Card);
        }

        // 9
        private CapabilityProbability GetThreeGuysCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 6)
            {
                return GetDefaultCapability(35 * DoubleEffectFactor);
            }

            // В остальных случаях атакуем
            return new CapabilityProbability(CardCapability.Attack, 40);
        }

        // 10
        private CapabilityProbability GetTimeMistressCapability()
        {
            if (_discardPile.AllCards.Count() <= 0)
                return new CapabilityProbability(CardCapability.Attack, 50);

            return GetDefaultCapability(30 * DoubleEffectFactor);
        }

        // 11
        private CapabilityProbability GetSharpSnakeCapability()
        {
            if (_isScarecrowMode)
                return new CapabilityProbability(CardCapability.Play, 0);

            // Пока не реализованно обозначение увиденных карт, нет смысла разыгрывать

            return new CapabilityProbability(CardCapability.Attack, 100);
        }

        // 12
        private CapabilityProbability GetImpArmyCapability()
        {
            // Почти всегда лучше разыграть
            return GetDefaultCapability(80 * DoubleEffectFactor);
        }

        // 13
        private CapabilityProbability GetCursedMarkCapability()
        {
            // Вообще, как будто сюда никогда не попадет
            return new CapabilityProbability(CardCapability.HandTransfer, Random.Range(-50, 50));
        }

        // 14
        private CapabilityProbability GetRushingMailmanCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return new CapabilityProbability(CardCapability.Attack, 10);

            if (_deck.IsHasCards(4) && IsDoubleEffect)
                return GetDefaultCapability(30 * DoubleEffectFactor);

            if (_deck.IsHasCards(3) && IsDoubleEffect)
                return GetDefaultCapability(30 * DoubleEffectFactor / 3 * 4);

            // Есть 2 карты - бери
            if (_deck.IsHasCards(2))
                return GetDefaultCapability(30);

            // Есть 1 карта, ну хз
            if (_deck.IsHasCards(1))
                return GetDefaultCapability(0);

            // Есть 0 карт, атакуй
            return new CapabilityProbability(CardCapability.Attack, 40);
        }

        // 15
        private CapabilityProbability GetSchemerCapability()
        {
            // Если вероятность разыграть карту мала, то играть не надо. Тут не учитывается Жижа эффект + отсутствие карт
            if (_deck.IsHasCards(3) == false)
                if (_handEnemy.AllCards.Count() <= 1)
                    return new CapabilityProbability(CardCapability.Attack, 20);

            // С другой стороны, если он его разыграет, значит карты хуета, значит на след ход после шанс разыграть не хуйню под двойными эффектами, пока уберу.
            //if (_workCard.CardName == CardName.TimeLord) // Если эффект идет через Повелителя времени, разгырывать на ещё раз смысла почти нет. Будем считать что нет
            //    return new CapabilityProbability(CardCapability.Attack, 10);

            return GetDefaultCapability(100);
        }

        // 16
        private CapabilityProbability GetMimeCapability()
        {
            // Если реализовать эффект не получится, атакуем
            if (_handPlayer.AllCards.Count() == 0 || _deck.IsHasCards(2) == false)
                return new CapabilityProbability(CardCapability.Attack, 70);

            return GetDefaultCapability(15 * DoubleEffectFactor);
        }

        // 18
        private CapabilityProbability GetTimeChildCapability()
        {
            // Если нет сожженных карт и карт в колоде меньше или равно 1, то разыгрывать смысла 0
            if (_fireRoot.IsHasCards(1) == false && _deck.IsHasCards(2) == false)
                return new CapabilityProbability(CardCapability.Attack, 60);

            // Если таки есть, разыгрывать смысл есть. Но мало
            return GetDefaultCapability(0);
        }

        // 19
        private CapabilityProbability GetUndergrounderCapability()
        {
            if (_isScarecrowMode)
                return new CapabilityProbability(CardCapability.Play, 0);
            // Пока не реализованно обозначение увиденных карт, нет смысла разыгрывать
            return new CapabilityProbability(CardCapability.Attack, 100);
        }

        // 20
        private CapabilityProbability GetRobinGoodCapability()
        {
            // Если в колоде нет карт, то и нечего думать
            if (_deck.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 50);

            if (_fireEffectHandlerEnemy.IsActive)
                return new CapabilityProbability(CardCapability.Attack, 10);

            int handCardsCountDiff = _handPlayer.AllCards.Count() - (_handEnemy.AllCards.Count() - 1);

            // Если по итогу не возьмем карт то и разыгрывать нет смысла
            if (handCardsCountDiff <= 0)
                return new CapabilityProbability(CardCapability.Attack, 30);

            int countDeck = _deck.AllCards.Count();

            if (countDeck < handCardsCountDiff)
                handCardsCountDiff = countDeck;

            // Чем больше карт, тем больше хотим взять
            return GetDefaultCapability((handCardsCountDiff - 1) * 10);
        }

        // 21
        private CapabilityProbability GetGeneralCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            //int maxConsecutive = freeNumbers.OrderBy(n => n).Select((num, index) => new { num, index }).GroupBy(x => x.num - x.index).Max(g => g.Count());
            int maxConsecutive = GetMaxConsecutive(freeNumbers, 4);

            if (freeNumbers.Count() >= 6)
            {
                if (maxConsecutive > 4)
                    maxConsecutive = 4;

                if (maxConsecutive < 1)
                    maxConsecutive = 1;

                if (maxConsecutive <= 1)
                    return new CapabilityProbability(CardCapability.Attack, 40);

                //if (maxConsecutive == 2)
                //    return GetDefaultCapability(0);

                //if (maxConsecutive == 3)
                //    return GetDefaultCapability(35);

                //return GetDefaultCapability(70);
                // ЗАМЕНИЛ ЧТО ВЫШЕ ЗАКОММИЧЕНО НА 1 СТРОКУ, НО ЕСЛИ 4 ИЗМЕНИТСЯ НАДО БУДЕТ ДУМАТЬ ДРУГУЮ ЛОГИКУ

                return GetDefaultCapability((35 * (maxConsecutive - 2)) * DoubleEffectFactor);
            }

            // В остальных случаях атакуем
            return new CapabilityProbability(CardCapability.Attack, 40);
        }

        private int GetMaxConsecutive(IEnumerable<int> freeNumbers, int consecutiveCount)
        {
            int firstNumber = GameSettings.DefaultCardNumbers.Min();
            int lastNumber = GameSettings.DefaultCardNumbers.Max();
            int maxConsecutive = 0;

            for (int startNumber = firstNumber; startNumber < lastNumber - (consecutiveCount - 1); startNumber++)
            {
                int localfindedConsecutive = 0;

                for (int i = 0; i < consecutiveCount; i++)
                {
                    if (freeNumbers.Contains(startNumber + i))
                        localfindedConsecutive++;
                }

                if (maxConsecutive < localfindedConsecutive)
                    maxConsecutive = localfindedConsecutive;
            }

            return maxConsecutive;
        }

        // 22
        private CapabilityProbability GetFateMistressCapability()
        {
            if (_handEnemy.AllCards.Count() - 1 <= _confirmableNumbersEnemy.FreeNumbers.Count())
                return new CapabilityProbability(CardCapability.Attack, 50);

            return GetDefaultCapability(80);
        }

        // 23
        private CapabilityProbability GetDumbMonkCapability()
        {
            if (IsDoubleEffect)
                return new CapabilityProbability(CardCapability.Attack, 10);

            if (_handEnemy.IsHasCards(2) == false)
                return new CapabilityProbability(CardCapability.Attack, 41);

            IReadOnlyList<Card> handCards = _handEnemy.AllCards.ToList();
            CardName firstCardName = handCards[0].CardName;

            if (firstCardName == CardName.CursedMark || firstCardName == CardName.PyromancersManuscript)
                return GetDefaultCapability(80);

            if (firstCardName == CardName.DumbMonk)
                if (handCards[1].CardName == CardName.CursedMark || handCards[1].CardName == CardName.PyromancersManuscript)
                    return GetDefaultCapability(80);

            return new CapabilityProbability(CardCapability.Attack, 70);
        }

        // 24
        private CapabilityProbability GetLeftEyedSisterCapability()
        {
            if (_handEnemy.IsHasCards(2) == false)
                return new CapabilityProbability(CardCapability.Attack, 10);

            int countCheckedNumbers = _confirmableNumbersPlayer.CheckedNumbers.Count();
            int extraCount = -30 + (countCheckedNumbers / 10) * 15;

            // Чем больше выбрано карт, тем больше шанс разыграть.
            int factor = Convert.ToInt32((100f / GameSettings.DefaultCardNumbers.Length) * countCheckedNumbers + extraCount);

            // Не ну если 0, то смысла прям вообще 0
            if (countCheckedNumbers == 0)
                return new CapabilityProbability(CardCapability.Attack, 5);

            return GetDefaultCapability(factor);
        }

        // 25
        private CapabilityProbability GetJusticeBullCapability()
        {
            int less25 = _confirmableNumbersEnemy.FreeNumbers.Where(n => n < 25).Count();
            int more25 = _confirmableNumbersEnemy.FreeNumbers.Where(n => n > 25).Count();
            int min;

            if (less25 > more25)
            {
                min = more25;
            }
            else
            {
                min = less25;
            }

            return GetDefaultCapability(min * 4);                
        }

        // 26
        private CapabilityProbability GetPatriarchCorallCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return new CapabilityProbability(CardCapability.Attack, 10);

            if (_deck.IsHasCards(6) && IsDoubleEffect)
                return GetDefaultCapability(30 * DoubleEffectFactor);

            if (_deck.IsHasCards(5) && IsDoubleEffect)
                return GetDefaultCapability(0 * DoubleEffectFactor);

            if (_deck.IsHasCards(4) && IsDoubleEffect)
                new CapabilityProbability(CardCapability.Attack, 10);

            // Есть 3 карты - бери
            if (_deck.IsHasCards(3))
                return GetDefaultCapability(30);

            // Есть 2 карты, ну хз
            if (_deck.IsHasCards(2))
                return GetDefaultCapability(0);

            // Есть 0-1 карт, атакуй
            return new CapabilityProbability(CardCapability.Attack, 60);
        }

        // 28
        private CapabilityProbability GetLittleBrotherCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return new CapabilityProbability(CardCapability.Attack, 5);

            if (_deck.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 55);

            const int StartCount = 1;
            int countCards = _brothersEffectHandlerEnemy.ExtraCount + StartCount;
            int countCardInDeck = _deck.AllCards.Count();

            if (countCardInDeck < countCards)
                countCards = countCardInDeck;

            return GetDefaultCapability(countCards * 15 * (DoubleEffectFactor * 3 / 2));
        }

        // 29
        private CapabilityProbability GetBrothersMotherCapability()
        {
            bool isOutOf_LittleBrother = _discardPile.Contains(28) || _fireRoot.Contains(28);
            bool isOutOf_MiddleBrother = _discardPile.Contains(38) || _fireRoot.Contains(38);
            bool isOutOf_BigBrother = _discardPile.Contains(48) || _fireRoot.Contains(48);

            if (isOutOf_LittleBrother && isOutOf_MiddleBrother && isOutOf_BigBrother)
            {
                return new CapabilityProbability(CardCapability.Attack, 40);
            }

            int summ = (1 - Convert.ToInt32(isOutOf_LittleBrother)) + (1 - Convert.ToInt32(isOutOf_MiddleBrother)) + (1 - Convert.ToInt32(isOutOf_BigBrother));

            return GetDefaultCapability(summ * 30 * DoubleEffectFactor);
        }

        // 30
        private CapabilityProbability GetScarecrowCapability()
        {
            if (_deck.IsHasCards(1) == false && _handPlayer.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 40);

            return GetDefaultCapability(60);
        }

        // 31
        private CapabilityProbability GetLuckyHorseshoeCapability()
        {
            return new CapabilityProbability(CardCapability.Attack, 10);
        }

        // 32
        private CapabilityProbability GetWiseMonkCapability()
        {
            IEnumerable<Card> tablePlayerCards = _tablePlayer.AllCards;
            if (tablePlayerCards.Count() > 0)
            {
                IEnumerable<CardName> tablePlayerCardsNames = tablePlayerCards.Select(c => c.CardName).Distinct();

                // Если 22 или 25 на столе, или 8 имитирующая одного из них, мы не хотим разыгрывать карту
                if (tablePlayerCardsNames.Contains(CardName.JusticeBull))
                    return new CapabilityProbability(CardCapability.Attack, 10);

                if (tablePlayerCardsNames.Contains(CardName.FateMistress))
                    return new CapabilityProbability(CardCapability.Attack, 10);

                if (tablePlayerCardsNames.Contains(CardName.TimeLord))
                {
                    // Если появятся новые карты, на пропуск карт например, это перестанет работать
                    if (_fateInevitabilityHandlerPlayer.IsActive)
                        return new CapabilityProbability(CardCapability.Attack, 10);

                    if (_skipTurnEffectHandlerPlayer.IsActive)
                        if (_tableEnemy.AllCards.Select(c => c.CardName).Contains(CardName.Mime) == false)
                            return new CapabilityProbability(CardCapability.Attack, 10);
                }

                // Если карта на столе есть, ээфективность разыгрывания возрастает
                return GetDefaultCapability(60);
            }

            if (_deck.IsHasCards(1) == false && _handPlayer.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 60);

            return GetDefaultCapability(0);
        }

        // 33
        private CapabilityProbability GetCowsHerdCapability()
        {
            //Нет никакого смысла атаковать
            if (_tableEnemy.IsHasCards(1) || _tablePlayer.IsHasCards(1))
                return GetDefaultCapability(80 * DoubleEffectFactor);

            return GetDefaultCapability(30 * DoubleEffectFactor);
        }

        // 34
        private CapabilityProbability GetHungryOgreCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return new CapabilityProbability(CardCapability.Attack, 10);

            if (_deck.IsHasCards(4) == false)
                return new CapabilityProbability(CardCapability.Attack, 40);

            if (_confirmableNumbersPlayer.FreeNumbers.Count() < 20)
                return new CapabilityProbability(CardCapability.Attack, 60);

            return GetDefaultCapability(15 * DoubleEffectFactor);
        }

        // 35
        private CapabilityProbability GetSharperCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return new CapabilityProbability(CardCapability.Attack, 10);

            if (_deck.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 50);

            if (_handEnemy.IsHasCards(2) == false)
                return new CapabilityProbability(CardCapability.Attack, 10);

            return GetDefaultCapability(25);
        }

        // 36
        private CapabilityProbability GetGunnerCapability()
        {
            return GetDefaultCapability(90 * DoubleEffectFactor);
        }

        // 38
        private CapabilityProbability GetMiddleBrotherCapability()
        {
            return GetDefaultCapability(90 * (DoubleEffectFactor * 3 / 2));
        }

        // 39
        private CapabilityProbability GetDeadOgreCapability()
        {
            return GetDefaultCapability(10 * DoubleEffectFactor);
        }

        // 40
        private CapabilityProbability GetOutOfControlBusCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 4)
            {
                return GetDefaultCapability(35 * DoubleEffectFactor);
            }

            // В остальных случаях атакуем
            return new CapabilityProbability(CardCapability.Attack, 30);
        }

        // 41
        private CapabilityProbability GetCursedMailmanCapability()
        {
            if (_deck.IsHasCards(4) && IsDoubleEffect)
                return GetDefaultCapability(35 * DoubleEffectFactor);

            if (_deck.IsHasCards(3) && IsDoubleEffect)
                return GetDefaultCapability(35 * DoubleEffectFactor * 4 / 3);

            if (_deck.IsHasCards(2) == false)
                return new CapabilityProbability(CardCapability.Attack, 70);

            if (_deck.IsHasCards(1))
                return GetDefaultCapability(0);

            return GetDefaultCapability(35);
        }

        // 42
        private CapabilityProbability GetRightEyedSisterCapability()
        {
            if (_deck.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 20);

            int countCheckedNumbers = _confirmableNumbersPlayer.CheckedNumbers.Count();

            int extraCount = -30 + (countCheckedNumbers / 10) * 15;

            // Чем больше выбрано карт, тем больше шанс разыграть.
            int factor = Convert.ToInt32((100f / GameSettings.DefaultCardNumbers.Length) * countCheckedNumbers + extraCount);

            // Не ну если 0, то смысла прям вообще 0
            if (countCheckedNumbers == 0)
                return new CapabilityProbability(CardCapability.Attack, 5);

            return GetDefaultCapability(factor);
        }

        // 43
        private CapabilityProbability GetStrongOgreCapability()
        {
            // Если карты противник не возьмет то и нечего думать
            if (_fireEffectHandlerPlayer.IsActive || _deck.IsHasCards(1) == false)
                return GetDefaultCapability(95 * DoubleEffectFactor);

            // Если возьмет, разыгрывать все равно крайне выгодно
            return GetDefaultCapability(25 * DoubleEffectFactor);
        }

        // 44
        private CapabilityProbability GetMafiaBossCapability()
        {
            if (_handPlayer.IsHasCards(1) == false)
                return new CapabilityProbability(CardCapability.Attack, 10);

            return GetDefaultCapability(70 * DoubleEffectFactor);
        }

        // 45
        private CapabilityProbability GetPyromancersManuscriptCapability()
        {
            return new CapabilityProbability(CardCapability.Attack, 65);
        }

        // 46
        private CapabilityProbability GetFalsePrinceCapability()
        {
            if (IsDoubleEffect)
                return new CapabilityProbability(CardCapability.Attack, 10);
            // Когда в декек мало карт осталось, уже не так выгодно
            if (_deck.IsHasCards(15) == false)
                return new CapabilityProbability(CardCapability.Attack, 60);

            return GetDefaultCapability(_deck.AllCards.Count() * 2);
        }

        // 48
        private CapabilityProbability GetBigBrotherCapability()
        {
            // Нет ситуации когда лучше атаковать (кроме сложных)
            if (_fireEffectHandlerEnemy.IsActive)
                return GetDefaultCapability(30 * (DoubleEffectFactor * 3 / 2));

            return GetDefaultCapability(91 * (DoubleEffectFactor * 3 / 2));
        }

        // 49
        private CapabilityProbability GetLastChanceCapability()
        {
            return GetDefaultCapability(10);
        }

        // 50
        private CapabilityProbability GetFallenGuardianCapability()
        {
            // Если невыгодных карт меньше или равно 4 и есть смысл разыгрывать, будем разыгрывать
            bool isGoodForFire = _handEnemy.AllCards.Where(c => c.CardName != CardName.CursedMark || c.CardName != CardName.PyromancersManuscript).Count() < 5;

            int countEvenNumbers = _confirmableNumbersEnemy.FreeNumbers.Where(c => c % 2 == 0).Count();
            int countOddNumbers = _confirmableNumbersEnemy.FreeNumbers.Where(c => c % 2 == 1).Count();

            bool isNeedPlay = countEvenNumbers > 7 && countOddNumbers > 7;

            if (isGoodForFire && isNeedPlay)
                return GetDefaultCapability(70);

            if (countEvenNumbers <= 1 || countOddNumbers <= 1)
                return new CapabilityProbability(CardCapability.Attack, 45);

            return GetDefaultCapability(0);
        }
    }
}