using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using Servers;
using Tools;
using Tools.UI;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceResultHandlerEnemyAI : ISelectResultHandler, ICompletable, IEnemyAIObject
    {
        private readonly InformationLabel _informationLabel;
        private readonly InformationLabelData _informationLabelData;
        private readonly FightProcessDBManager _fightProcessDBManager;

        private bool _isComplete;

        public ChoiceResultHandlerEnemyAI(InformationLabel informationLabel, InformationLabelData informationLabelData,
            FightProcessDBManager fightProcessDBManager)
        {
            _informationLabel = informationLabel;
            _informationLabelData = informationLabelData;
            _fightProcessDBManager = fightProcessDBManager;

            _isComplete = false;
        }

        public bool IsComplete => _isComplete;

        public void SetResult(SetSelectResultData data)
        {
            _isComplete = false;

            SettingResult(data).Forget();
        }

        private async UniTask SettingResult(SetSelectResultData data)
        {
            string selectResult = data.ResultType == ResultType.Success ? "SUCCESS" : data.ResultType == ResultType.Falled ? "FALLED" : throw new System.Exception("Unknown ResultType");

            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, false, null, selectResult, nameof(ChoiceResultHandlerEnemyAI));

            LabelActivateData labelActivateData = new LabelActivateData(_informationLabelData.DefaultInformationLabelText + data.Message);
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
            _informationLabel.Activate(informationLabelActivateData);

            await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: data.Token);

            _isComplete = true;
        }
    }
}