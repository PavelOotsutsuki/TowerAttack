using Cysharp.Threading.Tasks;
using GameFields.DiscardPiles;
using GameFields.InformationLabels;
using Servers;
using Tools.UI;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackResultHandlerEnemyAI : AttackResultHandler//, IPlayerObject
    {
        private readonly InformationLabel _informationLabel;
        private readonly InformationLabelData _informationLabelData;

        public AttackResultHandlerEnemyAI(DiscardPile discardPile, LoseActions loseActions,
            IAttackCardKeeper attackCardKeeper, AttackResultHandlerData data, InformationLabel informationLabel,
            InformationLabelData informationLabelData, FightProcessDBManager fightProcessDBManager) :
            base(discardPile, loseActions, attackCardKeeper, data, fightProcessDBManager)
        {
            _informationLabel = informationLabel;
            _informationLabelData = informationLabelData;
        }

        protected override bool? IsPlayersAction => false;
        protected override string GetName() => nameof(AttackResultHandlerEnemyAI);

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