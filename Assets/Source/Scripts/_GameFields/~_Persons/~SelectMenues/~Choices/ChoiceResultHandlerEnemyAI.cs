using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using Tools;
using Tools.UI;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceResultHandlerEnemyAI : ISelectResultHandler, ICompletable
    {
        private readonly InformationLabel _informationLabel;
        private readonly InformationLabelData _informationLabelData;

        private bool _isComplete;

        public ChoiceResultHandlerEnemyAI(InformationLabel informationLabel, InformationLabelData informationLabelData)
        {
            _informationLabel = informationLabel;
            _informationLabelData = informationLabelData;

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
            LabelActivateData labelActivateData = new LabelActivateData(_informationLabelData.DefaultInformationLabelText + data.Message);
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
            _informationLabel.Activate(informationLabelActivateData);

            await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: data.Token);

            _isComplete = true;
        }
    }
}