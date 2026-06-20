using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Effects;
using Cards.Sounds;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Histories;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.Discovers;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.Fires;
using GameFields.Persons.LookCardMenues;
using GameFields.Signals;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IPersonsState _personsState;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly InformationLabel _informationLabel;
        private readonly CardEffectConfig _voidEffectConfig;
        private readonly CardTransitManager _cardTransitManager;
        private readonly VariantCardCreator _variantCardCreator;
        private readonly BrothersEffectHandlerRoot _brothersEffectHandlerRoot;
        private readonly SignalBus _bus;
        private readonly PersonEffectsHandlerRoot _personEffectsHandlerRoot;
        private readonly DiscardManager _discardManager;
        private readonly LoseActionsRoot _loseActionsRoot;
        private readonly CardSoundRoot _cardSoundRoot;
        private readonly AwakeSoundReproducer _awakeSoundReproducer;
        private readonly ViewTransitTypesRoot _typesRoot;
        private readonly HistoryRoot _historyRoot;

        //private Effect _lastEffect;

        public EffectFactory(IPersonsState personsState, CardLocationViewRoot viewRoot, InformationLabel informationLabel,
            CardTransitManager cardTransitManager, VariantCardCreator variantCardCreator, BrothersEffectHandlerRoot brothersEffectHandler,
            SignalBus bus, PersonEffectsHandlerRoot personEffectsHandlerRoot, DiscardManager discardManager, LoseActionsRoot loseActionsRoot,
            CardSoundRoot cardSoundRoot, ViewTransitTypesRoot typesRoot, HistoryRoot historyRoot)
        {
            _personsState = personsState;
            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            _cardTransitManager = cardTransitManager;
            _variantCardCreator = variantCardCreator;
            _brothersEffectHandlerRoot = brothersEffectHandler;
            _bus = bus;
            _personEffectsHandlerRoot = personEffectsHandlerRoot;
            _discardManager = discardManager;
            _loseActionsRoot = loseActionsRoot;
            _cardSoundRoot = cardSoundRoot;
            _typesRoot = typesRoot;
            _historyRoot = historyRoot;

            _awakeSoundReproducer = new AwakeSoundReproducer(_cardSoundRoot, _viewRoot, typesRoot, _personsState);

            _voidEffectConfig = ScriptableObject.CreateInstance<CardEffectConfig>();
            //_voidEffectConfig = new CardEffectConfig();
            //_lastEffect = _voidEffect;
            //_voidEffect = new VoidEffect();
        }

        //public Effect Create(CardEffectConfig effectConfig, Action<int> callback)
        public void Create(CardEffectConfigPair cardEffectConfigPair)
        {
            CardSoundLogic currentCardSoundLogic = cardEffectConfigPair.CardEffectData.CardSoundLogic;

            _awakeSoundReproducer.Play(currentCardSoundLogic);

            bool isRememberEffect = true;
            CardEffectConfig currentEffectConfig = cardEffectConfigPair.CardEffectConfig;
            Effect effect;

            if (currentEffectConfig.Type == EffectType.TimeLord)
            {
                currentEffectConfig = _personsState.Deactive.LastEffect.Type == EffectType.TimeLord ? _voidEffectConfig : _personsState.Deactive.LastEffect;
            }

            //if (effectConfig.Type == EffectType.FateMistress)
            //{
            //    currentEffectConfig = PlayVariantEffect(_variantCardCreator.CreateByEffect(effectConfig.Type));
            //}
            if (_personsState.Active.IsWiseEffectActive)
            {
                isRememberEffect = false;
                currentEffectConfig = _voidEffectConfig;
            }
            else if (_personsState.Active.TryUseScarecrowEffect)
            {
                isRememberEffect = false;
                currentEffectConfig = _voidEffectConfig;
            }

            CardEffectConfigPair cardEffectConfigPairForCreateEffect = new CardEffectConfigPair(cardEffectConfigPair.Card, currentEffectConfig, cardEffectConfigPair.CardEffectData.CardSoundLogic);
            CardEffectConfigPair cardEffectConfigPairForSaveInPerson = new CardEffectConfigPair(cardEffectConfigPair.Card, cardEffectConfigPair.CardEffectConfig, cardEffectConfigPair.CardEffectData.CardSoundLogic);

            EffectDuration effectDuration = new EffectDuration();

            if (_personsState.Active.IsDoubleEffect)
            {
                //effect = new DoubleEffect(CreateEffect, currentEffectConfig, callback);
                effect = new DoubleEffect(CreateEffect, cardEffectConfigPairForCreateEffect, _bus, effectDuration, _personEffectsHandlerRoot,
                    _historyRoot, _personsState.Active);
            }
            else
            {
                //effect = CreateEffect(currentEffectConfig, callback);
                effect = CreateEffect(cardEffectConfigPairForCreateEffect, effectDuration);
            }

            // Тут эффект создает реально используемый, а конфиг должен быть разыгранной карты

            PersonEffect personEffect = new PersonEffect(effect, effectDuration, cardEffectConfigPairForSaveInPerson);
            //_personsState.Active.StartEffect(effect, effectConfig);
            _personsState.Active.StartEffect(personEffect, isRememberEffect);
            //_lastEffect = effect;

            //return effect;
        }

        //private Effect CreateEffect(CardEffectConfig effectConfig, Action<int> callback)
        private Effect CreateEffect(CardEffectConfigPair cardEffectConfigPair, EffectDuration effectDuration)
        {
            //return CreateEffect(effectConfig.Type, callback, effectConfig.Duration);
            return CreateEffect(cardEffectConfigPair.CardEffectConfig.Type, cardEffectConfigPair.CardEffectData, effectDuration);
        }

        //private Effect CreateEffect(EffectType effecType, Action<int> callback)
        //private Effect CreateEffect(EffectType effecType)
        //{
        //    //return CreateEffect(effecType, callback, 0);
        //    return CreateEffect(effecType, 0);
        //}

        //private Effect CreateEffect(EffectType effecType, Action<int> callback, int duration)
        private Effect CreateEffect(EffectType effecType, CardEffectData data, EffectDuration effectDuration)
        {
            EffectData effectData = new EffectData(_bus, data, effectDuration, _personEffectsHandlerRoot, _historyRoot, _personsState.Active);

            Effect effect = effecType switch
            {
                EffectType.Void => new VoidEffect(effectData),
                EffectType.Zhyzha => new ZhyzhaEffect(_personsState.Deactive, effectData),
                EffectType.Greedy => new GreedyEffect(_viewRoot, _cardTransitManager, effectData),
                EffectType.Pyromancer => new PyromancerEffect(_personsState.Deactive, effectData),
                EffectType.CoolBookmaker => new CoolBookmakerEffect(effectData),
                EffectType.BlindOldMan => new BlindOldManEffect(effectData),
                EffectType.DetectiveRhodes => new DetectiveRhodesEffect(_personsState.Deactive, _cardTransitManager,
                _viewRoot, _informationLabel, _typesRoot, effectData),
                EffectType.BlueGnome => new BlueGnomeEffect(effectData),
                EffectType.TimeLord => new VoidEffect(effectData),// new TimeLordEffect(_personsState.Deactive, this),
                EffectType.ThreeGuys => new ThreeGuysEffect(effectData),
                EffectType.TimeMistress => new TimeMistressEffect(_viewRoot, _cardTransitManager,
                _typesRoot, effectData),
                EffectType.SharpSnake => new SharpSnakeEffect(_personsState.Deactive, _viewRoot,
                _informationLabel, _typesRoot, effectData),
                EffectType.ImpArmy => new ImpArmyEffect(_personsState.Deactive, effectData),
                EffectType.CursedMark => new VoidEffect(effectData), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.RushingMailman => new RushingMailmanEffect(effectData),
                EffectType.Schemer => new SchemerEffect(_personsState.Deactive, effectData),
                EffectType.Mime => new MimeEffect(_personsState.Deactive, _viewRoot, _informationLabel,
                _typesRoot, effectData),
                EffectType.RedGnome => new RedGnomeEffect(effectData),
                EffectType.TimeChild => new TimeChildEffect(_viewRoot, _cardTransitManager, _typesRoot, effectData),
                EffectType.Undergrounder => new UndergrounderEffect(_viewRoot, _informationLabel, effectData),
                EffectType.RobinGood => new RobinGoodEffect(_personsState.Deactive, _viewRoot, effectData),
                EffectType.General => new GeneralEffect(effectData),
                EffectType.FateMistress => new FateMistressEffect(_variantCardCreator, CreateEffect, effecType, effectData),
                EffectType.FateMistress_FatefulAttack => new FateMistress_FatefulAttackEffect(_loseActionsRoot, _viewRoot,
                _typesRoot, effectData),
                EffectType.FateMistress_FateInevitability => new FateMistress_FateInevitabilityEffect(effectData),
                EffectType.DumbMonk => new DumbMonkEffect(_viewRoot, _cardTransitManager, _typesRoot, effectData),
                EffectType.LeftEyedSister => new LeftEyedSisterEffect(_viewRoot, _cardTransitManager,
                _typesRoot, effectData),
                EffectType.JusticeBull => new JusticeBullEffect(_variantCardCreator, CreateEffect, effecType,
                effectData),
                EffectType.JusticeBull_SmallerOnesArmy => new JusticeBull_SmallerOnesArmyEffect(_personsState.Deactive, _informationLabel,
                CreateEffect, effectData),
                EffectType.JusticeBull_BigOnesArmy => new JusticeBull_BigOnesArmyEffect(_personsState.Deactive, _informationLabel,
                CreateEffect, effectData),
                EffectType.JusticeBull_TrueChoiceEffect => new JusticeBull_TrueChoiceEffect(effectData),
                EffectType.JusticeBull_FalseChoiceEffect => new JusticeBull_FalseChoiceEffect(effectData),
                EffectType.PatriarchCorall => new PatriarchCorallEffect(_personsState.Deactive, _cardTransitManager,
                _typesRoot, effectData),
                EffectType.GreenGnome => new GreenGnomeEffect(effectData),
                EffectType.LittleBrother => new LittleBrotherEffect(_brothersEffectHandlerRoot, effectData),
                EffectType.BrothersMother => new BrothersMotherEffect(effectData),
                EffectType.Scarecrow => new ScarecrowEffect(_personsState.Deactive, effectData),
                EffectType.LuckyHorseshoe => new VoidEffect(effectData), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.WiseMonk => new WiseMonkEffect(_personsState.Deactive, _viewRoot, _discardManager, _personEffectsHandlerRoot,
                _typesRoot, effectData),
                EffectType.CowsHerd => new CowsHerdEffect(_viewRoot, effectData),
                EffectType.HungryOgre => new HungryOgreEffect(_variantCardCreator, CreateEffect, effecType, effectData),
                EffectType.HungryOgre_SilentSearch => new HungryOgre_SilentSearchEffect(_personsState.Deactive, effectData),
                EffectType.HungryOgre_HighProfileCrime => new HungryOgre_HighProfileCrimeEffect(_personsState.Deactive, effectData),
                EffectType.Sharper => new SharperEffect(_viewRoot, _cardTransitManager, _typesRoot, effectData),
                EffectType.Gunner => new GunnerEffect(_personsState.Deactive, _viewRoot, _cardTransitManager,
                _cardSoundRoot, _informationLabel, _typesRoot, effectData),
                EffectType.WhiteGnome => new WhiteGnomeEffect(effectData),
                EffectType.MiddleBrother => new MiddleBrotherEffect(_brothersEffectHandlerRoot, effectData),
                EffectType.DeadOgre => new DeadOgreEffect(_personsState.Deactive, effectData),
                EffectType.OutOfControlBus => new OutOfControlBusEffect(_personsState.Deactive, _viewRoot,
                _cardTransitManager, _typesRoot, effectData),
                EffectType.CursedMailman => new CursedMailmanEffect(_personsState.Deactive, effectData),
                EffectType.RightEyedSister => new RightEyedSisterEffect(_viewRoot, _cardTransitManager, effectData),
                EffectType.StrongOgre => new StrongOgreEffect(_variantCardCreator, CreateEffect, effecType, effectData),
                EffectType.StrongOgre_WeakBlow => new StrongOgre_WeakBlowEffect(_personsState.Deactive, effectData),
                EffectType.StrongOgre_StrongBlow => new StrongOgre_StrongBlowEffect(_personsState.Deactive, effectData),
                EffectType.MafiaBoss => new MafiaBossEffect(_personsState.Deactive, _viewRoot, _cardTransitManager,
                _typesRoot, effectData),
                EffectType.PyromancersManuscript => new VoidEffect(effectData),
                EffectType.FalsePrince => new FalsePrinceEffect(effectData),
                EffectType.BlackGnome => new BlackGnomeEffect(effectData),
                EffectType.BigBrother => new BigBrotherEffect(_brothersEffectHandlerRoot, effectData),
                EffectType.LastChance => new LastChanceEffect(_viewRoot, _cardTransitManager, _typesRoot, effectData),
                EffectType.FallenGuardian => new FallenGuardianEffect(_variantCardCreator, CreateEffect, effecType, effectData),
                EffectType.FallenGuardian_NightSight => new FallenGuardian_NightSightEffect(_personsState.Deactive, _informationLabel,
                CreateEffect, effectData),
                EffectType.FallenGuardian_HeightenedSenses => new FallenGuardian_HeightenedSensesEffect(_personsState.Deactive, _informationLabel,
                CreateEffect, effectData),
                EffectType.FallenGuardian_TrueChoiceEffect => new FallenGuardian_TrueChoiceEffect(effectData),
                EffectType.FallenGuardian_FalseChoiceEffect => new FallenGuardian_FalseChoiceEffect(_viewRoot,
                _cardTransitManager, _typesRoot, effectData),
                _ => throw new NullReferenceException("Effect is not founded")
            };

            //callback.Invoke(effect.Duration);
            return effect;
        }



        //private CardEffectConfig PlayVariantEffect(IReadOnlyList<VariantCard> variantCards)
        //{
        //    DiscoverResult discoverResult = new DiscoverResult();
        //    _personsState.Active.DiscoverCards(variantCards, "Выберите эффект", discoverResult);

        //    return WaitingUntilCreateEffect(discoverResult);
        //}

        //private CardEffectConfig WaitingUntilCreateEffect(DiscoverResult discoverResult)
        //{
        //    UniTask.WaitUntil(() => discoverResult.IsComplete);

        //    VariantCard variantCard = (VariantCard)discoverResult.Result;
        //    CardEffectConfig effectConfig = variantCard.EffectConfig;

        //    return effectConfig;
        //}
    }
}