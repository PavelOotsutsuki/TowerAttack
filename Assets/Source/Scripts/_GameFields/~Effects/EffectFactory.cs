using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Effects;
using Cards.Sounds;
using Cysharp.Threading.Tasks;
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

        //private Effect _lastEffect;

        public EffectFactory(IPersonsState personsState, CardLocationViewRoot viewRoot, InformationLabel informationLabel,
            CardTransitManager cardTransitManager, VariantCardCreator variantCardCreator, BrothersEffectHandlerRoot brothersEffectHandler,
            SignalBus bus, PersonEffectsHandlerRoot personEffectsHandlerRoot, DiscardManager discardManager, LoseActionsRoot loseActionsRoot,
            CardSoundRoot cardSoundRoot)
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
            _voidEffectConfig = ScriptableObject.CreateInstance<CardEffectConfig>();
            //_voidEffectConfig = new CardEffectConfig();
            //_lastEffect = _voidEffect;
            //_voidEffect = new VoidEffect();
        }

        //public Effect Create(CardEffectConfig effectConfig, Action<int> callback)
        public void Create(CardEffectConfigPair cardEffectConfigPair)
        {
            CardSoundLogic currentCardSoundLogic = cardEffectConfigPair.CardEffectData.CardSoundLogic;

            if (currentCardSoundLogic is IAwakeSoundKeeper awakeSoundKeeper)
                _cardSoundRoot.Play(awakeSoundKeeper.AwakeSound);

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
                effect = new DoubleEffect(CreateEffect, cardEffectConfigPairForCreateEffect, _bus, effectDuration, _personEffectsHandlerRoot);
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
                EffectType.SharpSnake => new SharpSnakeEffect(_personsState.Active, _viewRoot, _informationLabel, effectData),
                EffectType.ImpArmy => new ImpArmyEffect(_personsState.Deactive, effectData),
                EffectType.CursedMark => new VoidEffect(effectData), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.RushingMailman => new RushingMailmanEffect(_personsState.Active, effectData),
                EffectType.Schemer => new SchemerEffect(_personsState.Active, _personsState.Deactive, effectData),
                EffectType.Mime => new MimeEffect(_personsState.Active, _personsState.Deactive, _viewRoot, _informationLabel, effectData),
                EffectType.RedGnome => new RedGnomeEffect(_personsState.Active, effectData),
                EffectType.TimeChild => new TimeChildEffect(_viewRoot, _cardTransitManager, effectData),
                EffectType.Undergrounder => new UndergrounderEffect(_personsState.Active, _viewRoot, _informationLabel, effectData),
                EffectType.RobinGood => new RobinGoodEffect(_personsState.Active, _personsState.Deactive, _viewRoot, effectData),
                EffectType.General => new GeneralEffect(_personsState.Active, effectData),
                //EffectType.FateMistress => _personsState.Active is Player ?
                //new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback) :
                //new VoidEffect(),
                //EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, callback, effecType),
                EffectType.FateMistress => new FateMistressEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType, effectData),
                EffectType.FateMistress_FatefulAttack => new FateMistress_FatefulAttackEffect(_personsState.Active, _loseActionsRoot,
                _viewRoot, effectData),
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
                EffectType.LuckyHorseshoe => new VoidEffect(effectData), // Нельзя разыграть, мб стоит выдать экспшн
                EffectType.WiseMonk => new WiseMonkEffect(_personsState.Deactive, _viewRoot, _discardManager, _personEffectsHandlerRoot, effectData),
                EffectType.CowsHerd => new CowsHerdEffect(_personsState.Active, _viewRoot, effectData),
                EffectType.HungryOgre => new HungryOgreEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType,
                effectData),
                EffectType.HungryOgre_SilentSearch => new HungryOgre_SilentSearchEffect(_personsState.Active, _personsState.Deactive,
                effectData),
                EffectType.HungryOgre_HighProfileCrime => new HungryOgre_HighProfileCrimeEffect(_personsState.Active, _personsState.Deactive,
                effectData),
                EffectType.Sharper => new SharperEffect(_personsState.Active, _viewRoot, _cardTransitManager, effectData),
                EffectType.Gunner => new GunnerEffect(_personsState.Active, _viewRoot, _cardTransitManager,
                _cardSoundRoot, _informationLabel, effectData),
                EffectType.WhiteGnome => new WhiteGnomeEffect(_personsState.Active, effectData),
                EffectType.MiddleBrother => new MiddleBrotherEffect(_personsState.Active, _brothersEffectHandlerRoot, effectData),
                EffectType.DeadOgre => new DeadOgreEffect(_personsState.Deactive, effectData),
                EffectType.OutOfControlBus => new OutOfControlBusEffect(_personsState.Active, _viewRoot, _cardTransitManager,
                effectData),
                EffectType.CursedMailman => new CursedMailmanEffect(_personsState.Deactive, effectData),
                EffectType.RightEyedSister => new RightEyedSisterEffect(_personsState.Active, _viewRoot,
                _cardTransitManager, effectData),
                EffectType.StrongOgre => new StrongOgreEffect(_personsState.Active, _variantCardCreator, CreateEffect, effecType,
                effectData),
                EffectType.StrongOgre_WeakBlow => new StrongOgre_WeakBlowEffect(_personsState.Active, _personsState.Deactive,
                 effectData),
                EffectType.StrongOgre_StrongBlow => new StrongOgre_StrongBlowEffect(_personsState.Active, _personsState.Deactive,
                 effectData),
                EffectType.MafiaBoss => new MafiaBossEffect(_personsState.Active, _viewRoot, _cardTransitManager, effectData),
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