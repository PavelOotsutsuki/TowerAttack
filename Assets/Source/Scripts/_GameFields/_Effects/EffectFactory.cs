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

        //private readonly SignalBus _bus;
        //private Effect _lastEffect;

        public EffectFactory(IPersonsState personsState, CardLocationViewRoot viewRoot, InformationLabel informationLabel,
            CardTransitManager cardTransitManager, VariantCardCreator variantCardCreator, BrothersEffectHandlerRoot brothersEffectHandler)
        {
            _personsState = personsState;
            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            _cardTransitManager = cardTransitManager;
            _variantCardCreator = variantCardCreator;
            _brothersEffectHandlerRoot = brothersEffectHandler;
            _voidEffectConfig = ScriptableObject.CreateInstance<CardEffectConfig>();
            //_voidEffectConfig = new CardEffectConfig();
            //_voidEffect = new VoidEffect();
            //_lastEffect = _voidEffect;
            //_bus = bus;
        }

        public Effect Create(CardEffectConfig effectConfig, Action<int> callback)
        {
            CardEffectConfig currentEffectConfig = effectConfig;
            Effect effect;

            if (effectConfig.Type == EffectType.TimeLord)
            {
                currentEffectConfig = _personsState.Deactive.LastEffect.Type == EffectType.TimeLord ? _voidEffectConfig : _personsState.Deactive.LastEffect;
            }

            //if (effectConfig.Type == EffectType.FateMistress)
            //{
            //    currentEffectConfig = PlayVariantEffect(_variantCardCreator.CreateByEffect(effectConfig.Type));
            //}

            if (_personsState.Active.IsDoubleEffect)
            {
                effect = new DoubleEffect(CreateEffect, currentEffectConfig, callback);
            }
            else
            {
                effect = CreateEffect(currentEffectConfig, callback);
            }

            _personsState.Active.StartEffect(effect, effectConfig);
            //_lastEffect = effect;

            return effect;
        }

        private Effect CreateEffect(CardEffectConfig effectConfig, Action<int> callback)
        {
            return CreateEffect(effectConfig.Type, callback, effectConfig.Duration);
        }

        private Effect CreateEffect(EffectType effecType, Action<int> callback)
        {
            return CreateEffect(effecType, callback, 0);
        }

        private Effect CreateEffect(EffectType effecType, Action<int> callback, int duration)
        {
            Effect effect = effecType switch
            {
                EffectType.Void => new VoidEffect(),
                EffectType.Zhyzha => new ZhyzhaEffect(_personsState.Deactive, duration),
                //EffectType.Greedy => new GreedyEffect_OLD(_personsState.Active, _personsState.Deactive),
                EffectType.Greedy => new GreedyEffect(_viewRoot, _cardTransitManager),
                EffectType.Pyromancer => new PyromancerEffect(_personsState.Deactive, duration),
                EffectType.CoolBookmaker => new CoolBookmakerEffect(_personsState.Active),
                EffectType.BlindOldMan => new BlindOldManEffect(_personsState.Active),
                EffectType.DetectiveRhodes => new DetectiveRhodesEffect(_personsState.Active, _cardTransitManager, _viewRoot, _informationLabel),
                EffectType.BlueGnome => new BlueGnomeEffect(_personsState.Active),
                EffectType.TimeLord => new VoidEffect(),// new TimeLordEffect(_personsState.Deactive, this),
                EffectType.ThreeGuys => new ThreeGuysEffect(_personsState.Active),
                EffectType.TimeMistress => new TimeMistressEffect(_personsState.Active, _viewRoot, _cardTransitManager),
                EffectType.SharpSnake => new SharpSnakeEffect(_personsState.Active, _viewRoot),
                EffectType.ImpArmy => new ImpArmyEffect(_personsState.Deactive, duration),
                EffectType.CursedMark => new VoidEffect(), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.RushingMailman => new RushingMailmanEffect(_personsState.Active),
                EffectType.Schemer => new SchemerEffect(_personsState.Active, _personsState.Deactive, duration),
                EffectType.Mime => new MimeEffect(_personsState.Active, _personsState.Deactive, _viewRoot, _informationLabel),
                EffectType.RedGnome => new RedGnomeEffect(_personsState.Active),
                EffectType.TimeChild => new TimeChildEffect(_viewRoot, _cardTransitManager),
                EffectType.Undergrounder => new UndergrounderEffect(_personsState.Active, _viewRoot),
                EffectType.RobinGood => new RobinGoodEffect(_personsState.Active, _personsState.Deactive, _viewRoot),
                EffectType.General => new GeneralEffect(_personsState.Active),
                //EffectType.FateMistress => _personsState.Active is Player ?
                //new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback) :
                //new VoidEffect(),
                EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.FateMistress_FatefulAttack => new FateMistress_FatefulAttackEffect(_personsState.Active, _viewRoot),
                //EffectType.FateMistress_FatefulAttack => new FateMistress_FateInevitability(_personsState.Active, effectConfig.Duration),
                EffectType.FateMistress_FateInevitability => new FateMistress_FateInevitabilityEffect(_personsState.Active, duration),
                EffectType.DumbMonk => new DumbMonkEffect(_personsState.Active, _viewRoot, _cardTransitManager),
                EffectType.LeftEyedSister => new LeftEyedSisterEffect(_personsState.Active, _viewRoot, _cardTransitManager),
                EffectType.JusticeBull => new JusticeBullEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.JusticeBull_SmallerOnesArmy => new JusticeBull_SmallerOnesArmyEffect(_personsState.Deactive, _informationLabel,
                CreateEffect, callback, _personsState.Active),
                EffectType.JusticeBull_BigOnesArmy => new JusticeBull_BigOnesArmyEffect(_personsState.Deactive, _informationLabel,
                CreateEffect, callback, _personsState.Active),
                EffectType.JusticeBull_TrueChoiceEffect => new JusticeBull_TrueChoiceEffect(_personsState.Active),
                EffectType.JusticeBull_FalseChoiceEffect => new JusticeBull_FalseChoiceEffect(_personsState.Active),
                EffectType.PatriarchCorall => new PatriarchCorallEffect(_personsState.Active, _cardTransitManager),
                EffectType.GreenGnome => new GreenGnomeEffect(_personsState.Active),
                EffectType.LittleBrother => new LittleBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot),
                EffectType.BrothersMother => new VoidEffect(),
                EffectType.Scarecrow => new VoidEffect(),
                EffectType.LuckyHorseshoe => new VoidEffect(),
                EffectType.WiseMonk => new VoidEffect(),
                EffectType.CowsHerd => new VoidEffect(),
                EffectType.HungryOgre => new VoidEffect(),
                EffectType.Sharper => new VoidEffect(),
                EffectType.Gunner => new VoidEffect(),
                EffectType.WhiteGnome => new VoidEffect(),
                EffectType.MiddleBrother => new VoidEffect(),
                EffectType.DeadOgre => new VoidEffect(),
                EffectType.OutOfControlBus => new VoidEffect(),
                EffectType.CursedMailman => new VoidEffect(),
                EffectType.RightEyedSister => new VoidEffect(),
                EffectType.StrongOgre => new VoidEffect(),
                EffectType.MafiaBoss => new VoidEffect(),
                EffectType.PyromancersManuscript => new VoidEffect(),
                EffectType.FalsePrince => new VoidEffect(),
                EffectType.BlackGnome => new VoidEffect(),
                EffectType.BigBrother => new VoidEffect(),
                EffectType.LastChance => new VoidEffect(),
                EffectType.FallenGuardian => new VoidEffect(),

                _ => throw new NullReferenceException("Effect is not founded")
            };

            callback.Invoke(effect.Duration);
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