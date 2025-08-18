using System;
using Cards;
using GameFields.InformationLabels;
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
        //private readonly SignalBus _bus;
        //private Effect _lastEffect;

        public EffectFactory(IPersonsState personsState, CardLocationViewRoot viewRoot, InformationLabel informationLabel,
            CardTransitManager cardTransitManager/*, SignalBus bus*/)
        {
            _personsState = personsState;
            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            _cardTransitManager = cardTransitManager;
            _voidEffectConfig = ScriptableObject.CreateInstance<CardEffectConfig>();
            //_voidEffectConfig = new CardEffectConfig();
            //_voidEffect = new VoidEffect();
            //_lastEffect = _voidEffect;
            //_bus = bus;
        }

        public Effect Create(CardEffectConfig effectConfig)
        {
            Effect effect = effectConfig.Type switch
            {
                EffectType.Void => new VoidEffect(),
                EffectType.Zhyzha => new ZhyzhaEffect(_personsState.Deactive, effectConfig.Duration),
                //EffectType.Greedy => new GreedyEffect_OLD(_personsState.Active, _personsState.Deactive),
                EffectType.Greedy => new GreedyEffect(_viewRoot, _cardTransitManager),
                EffectType.Pyromancer => new PyromancerEffect(_personsState.Deactive, effectConfig.Duration),
                EffectType.CoolBookmaker => new CoolBookmakerEffect(_personsState.Active),
                EffectType.BlindOldMan => new BlindOldManEffect(_personsState.Active),
                EffectType.DetectiveRhodes => new DetectiveRhodesEffect(_personsState.Active, _cardTransitManager, _viewRoot, _informationLabel),
                EffectType.BlueGnome => new BlueGnomeEffect(_personsState.Active),
                EffectType.TimeLord => Create(_personsState.Deactive.LastEffect.Type == EffectType.TimeLord ? _voidEffectConfig : _personsState.Deactive.LastEffect),// new TimeLordEffect(_personsState.Deactive, this),
                EffectType.ThreeGuys => new ThreeGuysEffect(_personsState.Active),
                EffectType.TimeMistress => new TimeMistressEffect(_personsState.Active, _viewRoot, _cardTransitManager),
                EffectType.SharpSnake => new SharpSnakeEffect(_personsState.Deactive),
                EffectType.ImpArmy => new ImpArmyEffect(_personsState.Deactive, effectConfig.Duration),
                EffectType.CursedMark => new VoidEffect(),
                EffectType.RushingMailman => new VoidEffect(),
                EffectType.Schemer => new VoidEffect(),
                EffectType.Mime => new VoidEffect(),
                EffectType.RedGnome => new VoidEffect(),
                EffectType.TimeChild => new VoidEffect(),
                EffectType.Undergrounder => new VoidEffect(),
                EffectType.RobinGood => new VoidEffect(),
                EffectType.General => new VoidEffect(),
                EffectType.FateMistress => new VoidEffect(),
                EffectType.DumbMonk => new VoidEffect(),
                EffectType.LeftEyedSister => new VoidEffect(),
                EffectType.JusticeBull => new VoidEffect(),
                EffectType.PatriarchCorall => new PatriarchCorallEffect(_personsState.Active, _cardTransitManager),
                EffectType.GreenGnome => new VoidEffect(),
                EffectType.LittleBrother => new VoidEffect(),
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

            _personsState.Active.StartEffect(effect, effectConfig);
            //_lastEffect = effect;

            return effect;
        }

    }
}