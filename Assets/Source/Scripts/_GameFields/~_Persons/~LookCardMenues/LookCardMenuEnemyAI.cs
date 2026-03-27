using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuEnemyAI : ILookCardMenu
    {
        private bool _isComplete;

        private readonly InformationLabel _informationLabel;

        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        public LookCardMenuEnemyAI(InformationLabel informationLabel)
        {
            _informationLabel = informationLabel;
        }

        public void Activate(LookCardMenuActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _isComplete = false;

            Activating(data.LabelActivateData).ToUniTask();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
        }

        private IEnumerator Activating(LabelActivateData labelActivateData)
        {
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 10f);
            _informationLabel.Activate(informationLabelActivateData);

            yield return new WaitUntil(() => _informationLabel.IsComplete);

            _isComplete = true;

            Deactivate();
        }
    }
}
