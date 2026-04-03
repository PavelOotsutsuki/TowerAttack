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
        private readonly IEffectHandlerActiveWatcher _wiseMonkEffectHandler;
        private readonly IEffectHandlerActiveWatcher _scarecrowEffectHandler;

        public VeryHardAIThinkLogic(ICardWatcher cardWatcher, ICardView deck, ConfirmableNumbers confirmableNumbersEnemy,
            GnomeEffectHandler gnomeEffectHandler, ICardDropPlace cardPlayingZone, ICardView handEnemy, FireEffectHandler fireEffectHandlerEnemy,
            ICardView discardPile, ICardView fireRoot, ICardView handPlayer, IReadOnlyPersonEffectKeeper playerPersonEffectKeeper,
            ConfirmableNumbers confirmableNumbersPlayer, BrothersEffectHandler brothersEffectHandlerEnemy, Table tableEnemy,
            Table tablePlayer, SkipTurnEffectHandler skipTurnEffectHandlerPlayer, FateInevitabilityHandler fateInevitabilityHandlerPlayer,
            FireEffectHandler fireEffectHandlerPlayer, WiseMonkEffectHandler wiseMonkEffectHandler, ScarecrowEffectHandler scarecrowEffectHandler)
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
            _wiseMonkEffectHandler = wiseMonkEffectHandler;
            _scarecrowEffectHandler = scarecrowEffectHandler;
        }

        private int MaxGnomeCards => _cardWatcher.Cards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count();
        private int PlayedGnomeCards => _discardPile.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count()
            + _fireRoot.AllCards.Where(c => (c.CardCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice).Count();

        public CardCapability FindActionType(Card workCard)
        {
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
                    return CardCapability.Attack;

            // Если места на столе нет, или противником разыгран Мудрый Монах (32), карту точно не играем
            if (flags.Contains(CardCapability.Play))
                if (_cardPlayingZone.HasFreeSeat == false || _wiseMonkEffectHandler.IsActive)
                    flags.Remove(CardCapability.Play);

            if (flags.Count == 1)
                return flags[0];

            // Если противник разыграл Чучело (30), мы хотим его затриггерить как можно скорее (логика опциональна)
            if (flags.Contains(CardCapability.Play) && _scarecrowEffectHandler.IsActive)
                return CardCapability.Play;


            return FindCapabilityByEffect(workCard.EffectConfig.Type, flags);
        }

        private CardCapability FindCapabilityByEffect(EffectType effectType, IReadOnlyList<CardCapability> flags)
        {
            switch (effectType)
            {
                case EffectType.Void:
                    return CardCapability.Attack;
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

        /// <summary>
        /// minPlayChance - Каков шанс розыгрыша карты когда номеров осталось угадывать не так уж и много
        /// </summary>
        /// <param name="minPlayChance">Каков шанс розыгрыша карты когда номеров осталось угадывать не так уж и много</param>
        /// <returns></returns>
        private CardCapability GetDefaultCapability(int minPlayChance)
        {
            if (minPlayChance > 100)
                minPlayChance = 99;

            if (minPlayChance < 0)
                minPlayChance = 0;

            Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();
            // Тривиальный путь. Что лучше, разыграть карту или атаковать?
            // По большей части логика одна - чем больше неугаданных номеров, тем выше вероятность Play
            // Есть нюансы когда эффекты карты ссылают на то, чего нет, и поэтому ничего не делают, но это потом реализовать
            int countAll = _cardWatcher.Cards.Count;
            int countFree = _confirmableNumbersEnemy.FreeNumbers.Count();

            //Debug.Log($"{countAll}: countAll");
            //Debug.Log($"{countFree}: countFree");

            //int probabilityPlay = countFree * 100 / countAll;
            int probabilityPlay = minPlayChance + countFree * (100 - minPlayChance) / countAll;

            // Если включены двойные эффекты, эффективность розыгрыша уделичивается на 20. Для некоторых карт конечно ничего не меняется, но впадлу писать скрипт для всех отдельно
            if (_tableEnemy.AllCards.Select(c => c.CardName).Contains(CardName.Schemer) || _tablePlayer.AllCards.Select(c => c.CardName).Contains(CardName.Schemer))
                probabilityPlay += 20;

            if (probabilityPlay > 99)
                probabilityPlay = 99;

            int probabilityAttack = 100 - probabilityPlay;

            probability.Add(CardCapability.Play, probabilityPlay);
            probability.Add(CardCapability.Attack, probabilityAttack);
            return CalculateByProbability(probability);
        }

        // 7, 17, 27, 37, 47
        private CardCapability GetGnomeCapability(IReadOnlyList<CardCapability> flags)
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
            Debug.Log(debugMsg);
            #endregion

            Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();

            //Если GnomeChoice уже был разыгран
            if (_gnomeEffectHandler.CanActivate() == false)
            {
                probability.Add(CardCapability.Play, 5); // Берем минимальную вероятность, но все же берем

                int gnomeForgingProbability = 7; // По дефолту будет 7. Если мы разыграли GnomeChoice, то эффективность
                                                 // зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
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

                int gnomeForgingProbability = 15; // По дефолту будет 7. Если мы очень хотим разыграть,
                                                    // то эффективность ковки зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
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

                int gnomeForgingProbability = 35; // По дефолту будет 15. Если мы очень хотим разыграть,
                                                    // то эффективность ковки зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
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

                int gnomeForgingProbability = 55; // По дефолту будет 35. Если мы очень хотим разыграть,
                                                    // то эффективность ковки зависит только от того нужно ли брать карту

                if (_fireEffectHandlerEnemy.IsActive)
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

        // 1
        private CardCapability GetZhyzhaCapability()
        {
            Dictionary<CardCapability, int> probability = new Dictionary<CardCapability, int>();

            // Если карт нет, противник пропустит 2 хода, что весьма выгодно
            if (_deck.IsHasCards(1) == false)
                //|| _fireEffectHandler.IsFireMode) // Если fire убрал, тк не могу проверить уйдет ли сейчас пиромант в бито или нет. Да и толку мало
            {
                probability.Add(CardCapability.Play, 95);
                probability.Add(CardCapability.Attack, 5);
                return CalculateByProbability(probability);
            }

            // В остальных случаях обычная логика.
            return GetDefaultCapability(15);
        }

        // 2
        private CardCapability GetGreedyCapability()
        {
            // Пока все просто. Если у противника (player) больше карт чем у меня (enemy), значит я хочу обменяться. Иначе, атаковать.
            int enemyHandCardsCount = _handEnemy.AllCards.Count();
            int playerHandCardsCount = _handPlayer.AllCards.Count();

            if (enemyHandCardsCount < playerHandCardsCount - 1)
            {
                return GetDefaultCapability(35);
            }

            // В остальных случаях обычная логика.
            return GetDefaultCapability(15);
        }

        // 3
        private CardCapability GetPyromancerCapability()
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
            return CardCapability.Attack;
        }

        // 4 
        private CardCapability GetCoolBookmakerCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 5 && freeNumbers.Where(n => n % 2 == 0).Count() >= 3)
            {
                return GetDefaultCapability(35);
            }

            // В остальных случаях атакуем
            return CardCapability.Attack;
        }

        // 5
        private CardCapability GetBlindOldManCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 5 && freeNumbers.Where(n => n % 2 == 1).Count() >= 3)
            {
                return GetDefaultCapability(35);
            }

            // В остальных случаях атакуем
            return CardCapability.Attack;
        }

        // 6
        private CardCapability GetDetectiveRhodesCapability()
        {
            // Если нет карт ни в деке, ни в руке противника играть бессмысленно
            if (_handPlayer.AllCards.Count() == 0 || _deck.IsHasCards(1) == false)
                return CardCapability.Attack;

            // В остальных случаях базовая логика
            return GetDefaultCapability(20);
        }

        // 8
        private CardCapability GetTimeLordCapability()
        {
            if (_playerPersonEffectKeeper.PersonEffect == null)
                return CardCapability.Attack;

            if (_playerPersonEffectKeeper.PersonEffect.CardEffectConfig.Type == EffectType.TimeLord)
                return CardCapability.Attack;

            return FindActionType(_playerPersonEffectKeeper.PersonEffect.Card);
        }

        // 9
        private CardCapability GetThreeGuysCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 6)
            {
                return GetDefaultCapability(35);
            }

            // В остальных случаях атакуем
            return CardCapability.Attack;
        }

        // 10
        private CardCapability GetTimeMistressCapability()
        {
            if (_discardPile.AllCards.Count() <= 0)
                return CardCapability.Attack;

            return GetDefaultCapability(30);
        }

        // 11
        private CardCapability GetSharpSnakeCapability()
        {
            // Пока не реализованно обозначение увиденных карт, нет смысла разыгрывать
            return CardCapability.Attack;
        }

        // 12
        private CardCapability GetImpArmyCapability()
        {
            // Почти всегда лучше разыграть
            return GetDefaultCapability(60);
        }

        // 13
        private CardCapability GetCursedMarkCapability()
        {
            Debug.Log("Никогда не дойдет");
            // Вообще, как будто сюда никогда не попадет
            return CardCapability.HandTransfer;
        }

        // 14
        private CardCapability GetRushingMailmanCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return CardCapability.Attack;

            // Есть 2 карты - бери
            if (_deck.IsHasCards(2))
                return GetDefaultCapability(30);

            // Есть 1 карта, ну хз
            if (_deck.IsHasCards(1))
                return GetDefaultCapability(0);

            // Есть 0 карт, атакуй
            return CardCapability.Attack;
        }

        // 15
        private CardCapability GetSchemerCapability()
        {
            // Если вероятность разыграть карту мала, то играть не надо. Тут не учитывается Жижа эффект + отсутствие карт
            if (_deck.IsHasCards(3) == false)
                if (_handEnemy.AllCards.Count() <= 1)
                    return CardCapability.Attack;

            return GetDefaultCapability(20);
        }

        // 16
        private CardCapability GetMimeCapability()
        {
            // Если реализовать эффект не получится, атакуем
            if (_handPlayer.AllCards.Count() == 0 || _deck.IsHasCards(2) == false)
                return CardCapability.Attack;

            return GetDefaultCapability(15);
        }

        // 18
        private CardCapability GetTimeChildCapability()
        {
            // Если нет сожженных карт и карт в колоде меньше или равно 1, то разыгрывать смысла 0
            if (_fireRoot.IsHasCards(1) == false && _deck.IsHasCards(2) == false)
                return CardCapability.Attack;

            // Если таки есть, разыгрывать смысл есть. Но мало
            return GetDefaultCapability(50);
        }

        // 19
        private CardCapability GetUndergrounderCapability()
        {
            // Пока не реализованно обозначение увиденных карт, нет смысла разыгрывать
            return CardCapability.Attack;
        }

        // 20
        private CardCapability GetRobinGoodCapability()
        {
            // Если в колоде нет карт, то и нечего думать
            if (_deck.IsHasCards(1) == false)
                return CardCapability.Attack;

            if (_fireEffectHandlerEnemy.IsActive)
                return CardCapability.Attack;

            int handCardsCountDiff = _handPlayer.AllCards.Count() - (_handEnemy.AllCards.Count() - 1);

            // Если по итогу не возьмем карт то и разыгрывать нет смысла
            if (handCardsCountDiff <= 0)
                return CardCapability.Attack;

            int countDeck = _deck.AllCards.Count();

            if (countDeck < handCardsCountDiff)
                handCardsCountDiff = countDeck;

            // Чем больше карт, тем больше хотим взять
            return GetDefaultCapability((handCardsCountDiff - 1) * 10);
        }

        // 21
        private CardCapability GetGeneralCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            int maxConsecutive = freeNumbers.OrderBy(n => n).Select((num, index) => new { num, index }).GroupBy(x => x.num - x.index).Max(g => g.Count());

            if (freeNumbers.Count() >= 6)
            {
                if (maxConsecutive > 4)
                    maxConsecutive = 4;

                if (maxConsecutive < 1)
                    maxConsecutive = 1;

                if (maxConsecutive <= 1)
                    return CardCapability.Attack;

                //if (maxConsecutive == 2)
                //    return GetDefaultCapability(0);

                //if (maxConsecutive == 3)
                //    return GetDefaultCapability(35);

                //return GetDefaultCapability(70);
                // ЗАМЕНИЛ ЧТО ВЫШЕ ЗАКОММИЧЕНО НА 1 СТРОКУ, НО ЕСЛИ 4 ИЗМЕНИТСЯ НАДО БУДЕТ ДУМАТЬ ДРУГУЮ ЛОГИКУ

                return GetDefaultCapability(35 * (maxConsecutive - 2));
            }

            // В остальных случаях атакуем
            return CardCapability.Attack;
        }

        // 22
        private CardCapability GetFateMistressCapability()
        {
            if (_handEnemy.AllCards.Count() - 1 <= _confirmableNumbersEnemy.FreeNumbers.Count())
                return CardCapability.Attack;

            return GetDefaultCapability(80);
        }

        // 23
        private CardCapability GetDumbMonkCapability()
        {
            if (_handEnemy.IsHasCards(2) == false)
                return CardCapability.Attack;

            IReadOnlyList<Card> handCards = _handEnemy.AllCards.ToList();
            CardName firstCardName = handCards[0].CardName;

            if (firstCardName == CardName.CursedMark || firstCardName == CardName.PyromancersManuscript)
                return GetDefaultCapability(80);

            if (firstCardName == CardName.DumbMonk)
                if (handCards[1].CardName == CardName.CursedMark || handCards[1].CardName == CardName.PyromancersManuscript)
                    return GetDefaultCapability(80);

            return CardCapability.Attack;
        }

        // 24
        private CardCapability GetLeftEyedSisterCapability()
        {
            if (_handEnemy.IsHasCards(2) == false)
                return CardCapability.Attack;

            // Чем больше выбрано карт, тем больше шанс разыграть.
            int factor = Convert.ToInt32((100f / GameSettings.DefaultCardNumbers.Length) * _confirmableNumbersPlayer.CheckedNumbers.Count());

            return GetDefaultCapability(factor);
        }

        // 25
        private CardCapability GetJusticeBullCapability()
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
        private CardCapability GetPatriarchCorallCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return CardCapability.Attack;

            // Есть 3 карты - бери
            if (_deck.IsHasCards(3))
                return GetDefaultCapability(30);

            // Есть 2 карты, ну хз
            if (_deck.IsHasCards(2))
                return GetDefaultCapability(0);

            // Есть 0-1 карт, атакуй
            return CardCapability.Attack;
        }

        // 28
        private CardCapability GetLittleBrotherCapability()
        {
            if (_fireEffectHandlerEnemy.IsActive)
                return CardCapability.Attack;

            if (_deck.IsHasCards(1) == false)
                return CardCapability.Attack;

            const int StartCount = 1;
            int countCards = _brothersEffectHandlerEnemy.ExtraCount + StartCount;
            int countCardInDeck = _deck.AllCards.Count();

            if (countCardInDeck < countCards)
                countCards = countCardInDeck;

            return GetDefaultCapability(countCards);
        }

        // 29
        private CardCapability GetBrothersMotherCapability()
        {
            bool isOutOf_LittleBrother = _discardPile.Contains(28) || _fireRoot.Contains(28);
            bool isOutOf_MiddleBrother = _discardPile.Contains(38) || _fireRoot.Contains(38);
            bool isOutOf_BigBrother = _discardPile.Contains(48) || _fireRoot.Contains(48);

            if (isOutOf_LittleBrother && isOutOf_MiddleBrother && isOutOf_BigBrother)
            {
                return CardCapability.Attack;
            }

            int summ = Convert.ToInt32(isOutOf_LittleBrother) + Convert.ToInt32(isOutOf_MiddleBrother) + Convert.ToInt32(isOutOf_BigBrother);

            return GetDefaultCapability(summ * 30);
        }

        // 30
        private CardCapability GetScarecrowCapability()
        {
            if (_deck.IsHasCards(1) == false && _handPlayer.IsHasCards(1) == false)
                return CardCapability.Attack;

            return GetDefaultCapability(60);
        }

        // 31
        private CardCapability GetLuckyHorseshoeCapability()
        {
            Debug.Log("Никогда не дойдет");
            return CardCapability.Attack;
        }

        // 32
        private CardCapability GetWiseMonkCapability()
        {
            IEnumerable<Card> tablePlayerCards = _tablePlayer.AllCards;
            if (tablePlayerCards.Count() > 0)
            {
                IEnumerable<CardName> tablePlayerCardsNames = tablePlayerCards.Select(c => c.CardName).Distinct();

                // Если 22 или 25 на столе, или 8 имитирующая одного из них, мы не хотим разыгрывать карту
                if (tablePlayerCardsNames.Contains(CardName.JusticeBull))
                    return CardCapability.Attack;

                if (tablePlayerCardsNames.Contains(CardName.FateMistress))
                    return CardCapability.Attack;

                if (tablePlayerCardsNames.Contains(CardName.TimeLord))
                {
                    // Если появятся новые карты, на пропуск карт например, это перестанет работать
                    if (_fateInevitabilityHandlerPlayer.IsActive)
                        return CardCapability.Attack;

                    if (_skipTurnEffectHandlerPlayer.IsActive)
                        if (_tableEnemy.AllCards.Select(c => c.CardName).Contains(CardName.Mime) == false)
                            return CardCapability.Attack;
                }

                // Если карта на столе есть, ээфективность разыгрывания возрастает
                return GetDefaultCapability(60);
            }

            return GetDefaultCapability(0);
        }

        // 33
        private CardCapability GetCowsHerdCapability()
        {
            //Нет никакого смысла атаковать
            return CardCapability.Play;
        }

        // 34
        private CardCapability GetHungryOgreCapability()
        {
            if (_deck.IsHasCards(4) == false || _fireEffectHandlerEnemy.IsActive)
                return CardCapability.Attack;

            if (_confirmableNumbersPlayer.FreeNumbers.Count() < 20)
                return CardCapability.Attack;

            return GetDefaultCapability(15);
        }

        // 35
        private CardCapability GetSharperCapability()
        {
            if (_deck.IsHasCards(1) == false || _fireEffectHandlerEnemy.IsActive)
                return CardCapability.Attack;

            if (_handEnemy.IsHasCards(2) == false)
                return CardCapability.Attack;

            return GetDefaultCapability(25);
        }

        // 36
        private CardCapability GetGunnerCapability()
        {
            return CardCapability.Play;
        }

        // 38
        private CardCapability GetMiddleBrotherCapability()
        {
            return CardCapability.Play;
        }

        // 39
        private CardCapability GetDeadOgreCapability()
        {
            return GetDefaultCapability(0);
        }

        // 40
        private CardCapability GetOutOfControlBusCapability()
        {
            // Если есть карты, которые можно искать, Ищем
            IEnumerable<int> freeNumbers = _confirmableNumbersEnemy.FreeNumbers;

            if (freeNumbers.Count() >= 4)
            {
                return GetDefaultCapability(35);
            }

            // В остальных случаях атакуем
            return CardCapability.Attack;
        }

        // 41
        private CardCapability GetCursedMailmanCapability()
        {
            if (_deck.IsHasCards(2) == false)
                return CardCapability.Attack;

            if (_deck.IsHasCards(1))
                return GetDefaultCapability(0);

            return GetDefaultCapability(35);
        }

        // 42
        private CardCapability GetRightEyedSisterCapability()
        {
            if (_deck.IsHasCards(1) == false)
                return CardCapability.Attack;

            // Чем больше выбрано карт, тем больше шанс разыграть.
            int factor = Convert.ToInt32((100f / GameSettings.DefaultCardNumbers.Length) * _confirmableNumbersPlayer.CheckedNumbers.Count());

            return GetDefaultCapability(factor);
        }

        // 43
        private CardCapability GetStrongOgreCapability()
        {
            // Если карты противник не возьмет то и нечего думать
            if (_fireEffectHandlerPlayer.IsActive || _deck.IsHasCards(1) == false)
                return CardCapability.Play;

            // Если возьмет, разыгрывать все равно крайне выгодно
            return GetDefaultCapability(80);
        }

        // 44
        private CardCapability GetMafiaBossCapability()
        {
            if (_handPlayer.IsHasCards(1) == false)
                return CardCapability.Attack;

            return GetDefaultCapability(70);
        }

        // 45
        private CardCapability GetPyromancersManuscriptCapability()
        {
            Debug.Log("Никогда не дойдет");
            return CardCapability.Attack;
        }

        // 46
        private CardCapability GetFalsePrinceCapability()
        {
            // Когда в декек мало карт осталось, уже не так выгодно
            if (_deck.IsHasCards(15) == false)
                return CardCapability.Attack;

            return GetDefaultCapability(_deck.AllCards.Count());
        }

        // 48
        private CardCapability GetBigBrotherCapability()
        {
            // Нет ситуации когда лучше атаковать (кроме сложных)
            return CardCapability.Play;
        }

        // 49
        private CardCapability GetLastChanceCapability()
        {
            return GetDefaultCapability(10);
        }

        // 50
        private CardCapability GetFallenGuardianCapability()
        {
            // Если невыгодных карт меньше или равно 4 и есть смысл разыгрывать, будем разыгрывать
            bool isGoodForFire = _handEnemy.AllCards.Where(c => c.CardName != CardName.CursedMark || c.CardName != CardName.PyromancersManuscript).Count() < 5;

            int countEvenNumbers = _confirmableNumbersEnemy.FreeNumbers.Where(c => c % 2 == 0).Count();
            int countOddNumbers = _confirmableNumbersEnemy.FreeNumbers.Where(c => c % 2 == 1).Count();

            bool isNeedPlay = countEvenNumbers > 7 && countOddNumbers > 7;

            if (isGoodForFire && isNeedPlay)
                return GetDefaultCapability(70);

            if (countEvenNumbers <= 1 || countOddNumbers <= 1)
                return CardCapability.Attack;

            return GetDefaultCapability(0);
        }
    }
}