using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using GameFields.Persons.Commons;
using GameFields.Persons.Discovers;
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

        //private Effect _lastEffect;

        public EffectFactory(IPersonsState personsState, CardLocationViewRoot viewRoot, InformationLabel informationLabel,
            CardTransitManager cardTransitManager, VariantCardCreator variantCardCreator, BrothersEffectHandlerRoot brothersEffectHandler,
            SignalBus bus)
        {
            _personsState = personsState;
            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            _cardTransitManager = cardTransitManager;
            _variantCardCreator = variantCardCreator;
            _brothersEffectHandlerRoot = brothersEffectHandler;
            _bus = bus;
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

            if (_personsState.Active.IsScarecrowEffectActive)
            {
                isRememberEffect = false;
                currentEffectConfig = _voidEffectConfig;
            }

            CardEffectConfigPair trueCardEffectConfigPair = new CardEffectConfigPair(cardEffectConfigPair.Card, currentEffectConfig);

            if (_personsState.Active.IsDoubleEffect)
            {
                //effect = new DoubleEffect(CreateEffect, currentEffectConfig, callback);
                effect = new DoubleEffect(CreateEffect, trueCardEffectConfigPair, _bus);
            }
            else
            {
                //effect = CreateEffect(currentEffectConfig, callback);
                effect = CreateEffect(trueCardEffectConfigPair);
            }

            PersonEffect personEffect = new PersonEffect(effect, trueCardEffectConfigPair);
            //_personsState.Active.StartEffect(effect, effectConfig);
            _personsState.Active.StartEffect(personEffect, isRememberEffect);
            //_lastEffect = effect;

            //return effect;
        }

        //private Effect CreateEffect(CardEffectConfig effectConfig, Action<int> callback)
        private Effect CreateEffect(CardEffectConfigPair cardEffectConfigPair)
        {
            //return CreateEffect(effectConfig.Type, callback, effectConfig.Duration);
            return CreateEffect(cardEffectConfigPair.CardEffectConfig.Type, cardEffectConfigPair.CardEffectData);
        }

        //private Effect CreateEffect(EffectType effecType, Action<int> callback)
        //private Effect CreateEffect(EffectType effecType)
        //{
        //    //return CreateEffect(effecType, callback, 0);
        //    return CreateEffect(effecType, 0);
        //}

        //private Effect CreateEffect(EffectType effecType, Action<int> callback, int duration)
        private Effect CreateEffect(EffectType effecType, CardEffectData data)
        {
            Effect effect = effecType switch
            {
                EffectType.Void => new VoidEffect(_bus, data),
                EffectType.Zhyzha => new ZhyzhaEffect(_personsState.Deactive, _bus, data),
                EffectType.Greedy => new GreedyEffect(_viewRoot, _cardTransitManager, _bus, data),
                EffectType.Pyromancer => new PyromancerEffect(_personsState.Deactive, _bus, data),
                EffectType.CoolBookmaker => new CoolBookmakerEffect(_personsState.Active, _bus, data),
                EffectType.BlindOldMan => new BlindOldManEffect(_personsState.Active, _bus, data),
                EffectType.DetectiveRhodes => new DetectiveRhodesEffect(_personsState.Active, _cardTransitManager, _viewRoot,
                _informationLabel, _bus, data),
                EffectType.BlueGnome => new BlueGnomeEffect(_personsState.Active, _bus, data),
                EffectType.TimeLord => new VoidEffect(_bus, data),// new TimeLordEffect(_personsState.Deactive, this),
                EffectType.ThreeGuys => new ThreeGuysEffect(_personsState.Active, _bus, data),
                EffectType.TimeMistress => new TimeMistressEffect(_personsState.Active, _viewRoot, _cardTransitManager, _bus, data),
                EffectType.SharpSnake => new SharpSnakeEffect(_personsState.Active, _viewRoot, _bus, data),
                EffectType.ImpArmy => new ImpArmyEffect(_personsState.Deactive, _bus, data),
                EffectType.CursedMark => new VoidEffect(_bus, data), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.RushingMailman => new RushingMailmanEffect(_personsState.Active, _bus, data),
                EffectType.Schemer => new SchemerEffect(_personsState.Active, _personsState.Deactive, _bus, data),
                EffectType.Mime => new MimeEffect(_personsState.Active, _personsState.Deactive, _viewRoot, _informationLabel, _bus, data),
                EffectType.RedGnome => new RedGnomeEffect(_personsState.Active, _bus, data),
                EffectType.TimeChild => new TimeChildEffect(_viewRoot, _cardTransitManager, _bus, data),
                EffectType.Undergrounder => new UndergrounderEffect(_personsState.Active, _viewRoot, _bus, data),
                EffectType.RobinGood => new RobinGoodEffect(_personsState.Active, _personsState.Deactive, _viewRoot, _bus, data),
                EffectType.General => new GeneralEffect(_personsState.Active, _bus, data),
                //EffectType.FateMistress => _personsState.Active is Player ?
                //new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback) :
                //new VoidEffect(),
                //EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType, _bus, data),
                EffectType.FateMistress_FatefulAttack => new FateMistress_FatefulAttackEffect(_personsState.Active, _viewRoot, _bus, data),
                //EffectType.FateMistress_FatefulAttack => new FateMistress_FateInevitability(_personsState.Active, effectConfig.Duration),
                EffectType.FateMistress_FateInevitability => new FateMistress_FateInevitabilityEffect(_personsState.Active, _bus, data),
                EffectType.DumbMonk => new DumbMonkEffect(_personsState.Active, _viewRoot, _cardTransitManager, _bus, data),
                EffectType.LeftEyedSister => new LeftEyedSisterEffect(_personsState.Active, _viewRoot, _cardTransitManager, _bus, data),
                //EffectType.JusticeBull => new JusticeBullEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.JusticeBull => new JusticeBullEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType,
                _bus, data),
                EffectType.JusticeBull_SmallerOnesArmy => new JusticeBull_SmallerOnesArmyEffect(_personsState.Deactive, _informationLabel,
                //CreateEffect, callback, _personsState.Active),
                CreateEffect, _personsState.Active, _bus, data),
                EffectType.JusticeBull_BigOnesArmy => new JusticeBull_BigOnesArmyEffect(_personsState.Deactive, _informationLabel,
                //CreateEffect, callback, _personsState.Active),
                CreateEffect, _personsState.Active, _bus, data),
                EffectType.JusticeBull_TrueChoiceEffect => new JusticeBull_TrueChoiceEffect(_personsState.Active, _bus, data),
                EffectType.JusticeBull_FalseChoiceEffect => new JusticeBull_FalseChoiceEffect(_personsState.Active, _bus, data),
                EffectType.PatriarchCorall => new PatriarchCorallEffect(_personsState.Active, _cardTransitManager, _bus, data),
                EffectType.GreenGnome => new GreenGnomeEffect(_personsState.Active, _bus, data),
                EffectType.LittleBrother => new LittleBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, _bus, data),
                EffectType.BrothersMother => new BrothersMotherEffect(_personsState.Active, _bus, data),
                EffectType.Scarecrow => new ScarecrowEffect(_personsState.Deactive, _bus, data),
                EffectType.LuckyHorseshoe => new VoidEffect(_bus, data),
                EffectType.WiseMonk => new VoidEffect(_bus, data),
                EffectType.CowsHerd => new VoidEffect(_bus, data),
                EffectType.HungryOgre => new VoidEffect(_bus, data),
                EffectType.Sharper => new VoidEffect(_bus, data),
                EffectType.Gunner => new VoidEffect(_bus, data),
                EffectType.WhiteGnome => new VoidEffect(_bus, data),
                EffectType.MiddleBrother => new MiddleBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, _bus, data),
                EffectType.DeadOgre => new VoidEffect(_bus, data),
                EffectType.OutOfControlBus => new VoidEffect(_bus, data),
                EffectType.CursedMailman => new VoidEffect(_bus, data),
                EffectType.RightEyedSister => new VoidEffect(_bus, data),
                EffectType.StrongOgre => new VoidEffect(_bus, data),
                EffectType.MafiaBoss => new VoidEffect(_bus, data),
                EffectType.PyromancersManuscript => new VoidEffect(_bus, data),
                EffectType.FalsePrince => new VoidEffect(_bus, data),
                EffectType.BlackGnome => new VoidEffect(_bus, data),
                EffectType.BigBrother => new BigBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, _bus, data),
                EffectType.LastChance => new VoidEffect(_bus, data),
                EffectType.FallenGuardian => new VoidEffect(_bus, data),

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