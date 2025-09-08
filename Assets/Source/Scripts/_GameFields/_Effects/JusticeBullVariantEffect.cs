using System;
using System.Collections;
using Cards;
using GameFields.InformationLabels;
using GameFields.Persons.Commons;
using Tools.UI;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public abstract class JusticeBullVariantEffect : Effect
    {
        private const string TrueChoiceMessage = "ВЕРНО";
        private const string FalseChoiceMessage = "НЕВЕРНО";

        private readonly Person _activePerson;

        private readonly InformationLabel _informationLabel;
        private readonly Func<EffectType, CardEffectData, Effect> _effectCreator;
        private readonly CardEffectData _data;
        //private readonly Action<int> _callback;

        //public JusticeBullVariantEffect(InformationLabel informationLabel,
        //    Func<EffectType, Action<int>, Effect> effectCreator, Action<int> callback, Person activePerson) : base()
        public JusticeBullVariantEffect(InformationLabel informationLabel,
            Func<EffectType, CardEffectData, Effect> effectCreator, Person activePerson, SignalBus bus, CardEffectData data)
            : base(bus, data)
        {
            _informationLabel = informationLabel;
            _effectCreator = effectCreator;
            _data = data;
            //_callback = callback;
            _activePerson = activePerson;
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект ВАРИАНТ Быка правосудия закончен");
        }

        protected abstract bool IsTrueChoice();

        protected override IEnumerator OnPlaying()
        {
            Effect choiceEffect;

            if (IsTrueChoice())
            {
                LabelActivateData labelActivateData = new LabelActivateData(TrueChoiceMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);

                _informationLabel.Activate(informationLabelActivateData);

                yield return new WaitUntil(() => _informationLabel.IsComplete);

                //choiceEffect = _effectCreator.Invoke(EffectType.JusticeBull_TrueChoiceEffect, _callback);
                choiceEffect = _effectCreator.Invoke(EffectType.JusticeBull_TrueChoiceEffect, _data);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(FalseChoiceMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);

                _informationLabel.Activate(informationLabelActivateData);

                yield return new WaitUntil(() => _informationLabel.IsComplete);

                //choiceEffect = _effectCreator.Invoke(EffectType.JusticeBull_FalseChoiceEffect, _callback);
                choiceEffect = _effectCreator.Invoke(EffectType.JusticeBull_FalseChoiceEffect, _data);
            }

            _activePerson.ActivateJusticeBullEffect();

            yield return new WaitUntil(() => choiceEffect.IsComplete);
        }
    }
}