using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using GameFields.Persons.Commons;
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

        //private Effect _lastEffect;

        public EffectFactory(IPersonsState personsState, CardLocationViewRoot viewRoot, InformationLabel informationLabel,
            CardTransitManager cardTransitManager, VariantCardCreator variantCardCreator, BrothersEffectHandlerRoot brothersEffectHandler,
            SignalBus bus, PersonEffectsHandlerRoot personEffectsHandlerRoot)
        {
            _personsState = personsState;
            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            _cardTransitManager = cardTransitManager;
            _variantCardCreator = variantCardCreator;
            _brothersEffectHandlerRoot = brothersEffectHandler;
            _bus = bus;
            _personEffectsHandlerRoot = personEffectsHandlerRoot;
            _voidEffectConfig = ScriptableObject.CreateInstance<CardEffectConfig>();
            //_voidEffectConfig = new CardEffectConfig();
            //_lastEffect = _voidEffect;
            //_voidEffect = new VoidEffect();
        }

        //public Effect Create(CardEffectConfig effectConfig, Action<int> callback)
        public void Create(CardEffectConfigPair cardEffectConfigPair)
        {
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

            if (_personsState.Active.TryUseScarecrowEffect)
            {
                isRememberEffect = false;
                currentEffectConfig = _voidEffectConfig;
            }

            CardEffectConfigPair trueCardEffectConfigPair = new CardEffectConfigPair(cardEffectConfigPair.Card, currentEffectConfig);
            EffectDuration effectDuration = new EffectDuration();

            if (_personsState.Active.IsDoubleEffect)
            {
                //effect = new DoubleEffect(CreateEffect, currentEffectConfig, callback);
                effect = new DoubleEffect(CreateEffect, trueCardEffectConfigPair, _bus, effectDuration, _personEffectsHandlerRoot);
            }
            else
            {
                //effect = CreateEffect(currentEffectConfig, callback);
                effect = CreateEffect(trueCardEffectConfigPair, effectDuration);
            }

            PersonEffect personEffect = new PersonEffect(effect, effectDuration, trueCardEffectConfigPair);
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
            EffectData effectData = new EffectData(_bus, data, effectDuration, _personEffectsHandlerRoot);

            Effect effect = effecType switch
            {
                EffectType.Void => new VoidEffect(effectData),
                EffectType.Zhyzha => new ZhyzhaEffect(_personsState.Deactive, effectData),
                EffectType.Greedy => new GreedyEffect(_viewRoot, _cardTransitManager, effectData),
                EffectType.Pyromancer => new PyromancerEffect(_personsState.Deactive, effectData),
                EffectType.CoolBookmaker => new CoolBookmakerEffect(_personsState.Active, effectData),
                EffectType.BlindOldMan => new BlindOldManEffect(_personsState.Active, effectData),
                EffectType.DetectiveRhodes => new DetectiveRhodesEffect(_personsState.Active, _cardTransitManager, _viewRoot,
                _informationLabel, effectData),
                EffectType.BlueGnome => new BlueGnomeEffect(_personsState.Active, effectData),
                EffectType.TimeLord => new VoidEffect(effectData),// new TimeLordEffect(_personsState.Deactive, this),
                EffectType.ThreeGuys => new ThreeGuysEffect(_personsState.Active, effectData),
                EffectType.TimeMistress => new TimeMistressEffect(_personsState.Active, _viewRoot, _cardTransitManager, effectData),
                EffectType.SharpSnake => new SharpSnakeEffect(_personsState.Active, _viewRoot, effectData),
                EffectType.ImpArmy => new ImpArmyEffect(_personsState.Deactive, effectData),
                EffectType.CursedMark => new VoidEffect(effectData), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.RushingMailman => new RushingMailmanEffect(_personsState.Active, effectData),
                EffectType.Schemer => new SchemerEffect(_personsState.Active, _personsState.Deactive, effectData),
                EffectType.Mime => new MimeEffect(_personsState.Active, _personsState.Deactive, _viewRoot, _informationLabel, effectData),
                EffectType.RedGnome => new RedGnomeEffect(_personsState.Active, effectData),
                EffectType.TimeChild => new TimeChildEffect(_viewRoot, _cardTransitManager, effectData),
                EffectType.Undergrounder => new UndergrounderEffect(_personsState.Active, _viewRoot, effectData),
                EffectType.RobinGood => new RobinGoodEffect(_personsState.Active, _personsState.Deactive, _viewRoot, effectData),
                EffectType.General => new GeneralEffect(_personsState.Active, effectData),
                //EffectType.FateMistress => _personsState.Active is Player ?
                //new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback) :
                //new VoidEffect(),
                //EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType, effectData),
                EffectType.FateMistress_FatefulAttack => new FateMistress_FatefulAttackEffect(_personsState.Active, _viewRoot, effectData),
                //EffectType.FateMistress_FatefulAttack => new FateMistress_FateInevitability(_personsState.Active, effectConfig.Duration),
                EffectType.FateMistress_FateInevitability => new FateMistress_FateInevitabilityEffect(_personsState.Active, effectData),
                EffectType.DumbMonk => new DumbMonkEffect(_personsState.Active, _viewRoot, _cardTransitManager, effectData),
                EffectType.LeftEyedSister => new LeftEyedSisterEffect(_personsState.Active, _viewRoot, _cardTransitManager, effectData),
                //EffectType.JusticeBull => new JusticeBullEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.JusticeBull => new JusticeBullEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType,
                effectData),
                EffectType.JusticeBull_SmallerOnesArmy => new JusticeBull_SmallerOnesArmyEffect(_personsState.Deactive, _informationLabel,
                //CreateEffect, callback, _personsState.Active),
                CreateEffect, _personsState.Active, effectData),
                EffectType.JusticeBull_BigOnesArmy => new JusticeBull_BigOnesArmyEffect(_personsState.Deactive, _informationLabel,
                //CreateEffect, callback, _personsState.Active),
                CreateEffect, _personsState.Active, effectData),
                EffectType.JusticeBull_TrueChoiceEffect => new JusticeBull_TrueChoiceEffect(effectData),
                EffectType.JusticeBull_FalseChoiceEffect => new JusticeBull_FalseChoiceEffect(_personsState.Active, effectData),
                EffectType.PatriarchCorall => new PatriarchCorallEffect(_personsState.Active, _cardTransitManager, effectData),
                EffectType.GreenGnome => new GreenGnomeEffect(_personsState.Active, effectData),
                EffectType.LittleBrother => new LittleBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, effectData),
                EffectType.BrothersMother => new BrothersMotherEffect(_personsState.Active, effectData),
                EffectType.Scarecrow => new ScarecrowEffect(_personsState.Deactive, effectData),
                EffectType.LuckyHorseshoe => new VoidEffect(effectData),
                EffectType.WiseMonk => new VoidEffect(effectData),
                EffectType.CowsHerd => new VoidEffect(effectData),
                EffectType.HungryOgre => new VoidEffect(effectData),
                EffectType.Sharper => new VoidEffect(effectData),
                EffectType.Gunner => new VoidEffect(effectData),
                EffectType.WhiteGnome => new VoidEffect(effectData),
                EffectType.MiddleBrother => new MiddleBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, effectData),
                EffectType.DeadOgre => new VoidEffect(effectData),
                EffectType.OutOfControlBus => new VoidEffect(effectData),
                EffectType.CursedMailman => new VoidEffect(effectData),
                EffectType.RightEyedSister => new VoidEffect(effectData),
                EffectType.StrongOgre => new VoidEffect(effectData),
                EffectType.MafiaBoss => new VoidEffect(effectData),
                EffectType.PyromancersManuscript => new VoidEffect(effectData),
                EffectType.FalsePrince => new VoidEffect(effectData),
                EffectType.BlackGnome => new VoidEffect(effectData),
                EffectType.BigBrother => new BigBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, effectData),
                EffectType.LastChance => new VoidEffect(effectData),
                EffectType.FallenGuardian => new VoidEffect(effectData),

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