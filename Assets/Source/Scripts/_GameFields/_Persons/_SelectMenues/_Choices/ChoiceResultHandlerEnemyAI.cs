using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using Tools.UI;
using UnityEngine;

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

            SettingResult(data).ToUniTask();
        }

        private IEnumerator SettingResult(SetSelectResultData data)
        {
            LabelActivateData informationLableData = new LabelActivateData(_informationLabelData.DefaultInformationLabelText + data.Message);
            _informationLabel.Activate(informationLableData);

            yield return new WaitForSeconds(_informationLabelData.TimeViewInformationLabel);
            _informationLabel.Deactivate();

            yield return new WaitUntil(() => _informationLabel.IsComplete);

            _isComplete = true;
        }
    }
}