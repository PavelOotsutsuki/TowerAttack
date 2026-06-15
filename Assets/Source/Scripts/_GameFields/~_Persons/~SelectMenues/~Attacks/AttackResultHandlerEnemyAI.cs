using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using Tools.UI;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackResultHandlerEnemyAI : AttackResultHandler, IPlayerObject
    {
        private readonly InformationLabel _informationLabel;
        private readonly InformationLabelData _informationLabelData;

        public AttackResultHandlerEnemyAI(DiscardPile discardPile, LoseActions loseActions,
            IAttackCardKeeper attackCardKeeper, AttackResultHandlerData data, InformationLabel informationLabel,
            InformationLabelData informationLabelData) :
            base(discardPile, loseActions, attackCardKeeper, data)
        {
            _informationLabel = informationLabel;
            _informationLabelData = informationLabelData;
        }

        protected override async UniTask OnSettingResult(SetSelectResultData data)
        {
            await base.OnSettingResult(data);

            LabelActivateData labelActivateData = new LabelActivateData(_informationLabelData.DefaultInformationLabelText + data.Message);
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
            _informationLabel.Activate(informationLabelActivateData);

            await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: data.Token);
        }
    }
}