using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.InformationLabels;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using Tools.UI;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackResultHandlerEnemyAI : AttackResultHandler, IEnemyAIObject
    {
        private readonly InformationLabel _informationLabel;
        private readonly InformationLabelData _informationLabelData;

        public AttackResultHandlerEnemyAI(DiscardPile discardPile, SignalBus bus, IBoomTower tower,
            IAttackCardKeeper attackCardKeeper, AttackResultHandlerData data, InformationLabel informationLabel,
            InformationLabelData informationLabelData) :
            base(discardPile, bus, tower, attackCardKeeper, data)
        {
            _informationLabel = informationLabel;
            _informationLabelData = informationLabelData;
        }

        protected override IEnumerator OnSettingResult(SetSelectResultData data)
        {
            yield return base.OnSettingResult(data);

            LabelActivateData informationLableData = new LabelActivateData(_informationLabelData.DefaultInformationLabelText + data.Message);
            _informationLabel.Activate(informationLableData);

            yield return new WaitForSeconds(_informationLabelData.TimeViewInformationLabel);
            _informationLabel.Deactivate();

            yield return new WaitUntil(() => _informationLabel.IsComplete);
        }
    }
}