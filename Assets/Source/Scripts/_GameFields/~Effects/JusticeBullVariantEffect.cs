using System;
using Cards.Effects;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using GameFields.Persons;
using Tools.UI;

namespace GameFields.Effects
{
    public abstract class JusticeBullVariantEffect : Effect
    {
        private const string TrueChoiceMessage = "ВЕРНО";
        private const string FalseChoiceMessage = "НЕВЕРНО";

        private readonly Person _activePerson;

        private readonly InformationLabel _informationLabel;
        private readonly Func<EffectType, CardEffectData, EffectDuration, Effect> _effectCreator;
        private readonly CardEffectData _cardEffectData;
        private readonly EffectDuration _effectDuration;
        //private readonly Action<int> _callback;

        public JusticeBullVariantEffect(InformationLabel informationLabel,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectData data)
            : base(data, 0f)
        {
            _informationLabel = informationLabel;
            _effectCreator = effectCreator;
            _cardEffectData = data.CardEffectData;
            _effectDuration = data.EffectDuration;
            //_callback = callback;
            _activePerson = data.ActivePerson;
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект ВАРИАНТ Быка правосудия закончен");
        //}

        protected abstract bool IsTrueChoice();

        protected override async UniTask OnPlaying()
        {
            Effect choiceEffect;

            if (IsTrueChoice())
            {
                LabelActivateData labelActivateData = new LabelActivateData(TrueChoiceMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);

                _informationLabel.Activate(informationLabelActivateData);
                await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: Token);

                choiceEffect = _effectCreator.Invoke(EffectType.JusticeBull_TrueChoiceEffect, new CardEffectData(_cardEffectData.Card, 0, null), _effectDuration);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(FalseChoiceMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);

                _informationLabel.Activate(informationLabelActivateData);

                await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: Token);

                choiceEffect = _effectCreator.Invoke(EffectType.JusticeBull_FalseChoiceEffect, new CardEffectData(_cardEffectData.Card, 2, null), _effectDuration);
            }

            _activePerson.ActivateJusticeBullEffect();

            await UniTask.WaitUntil(() => choiceEffect.IsComplete, cancellationToken: Token);
        }
    }
}