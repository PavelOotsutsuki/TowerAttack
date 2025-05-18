using Tools;
using Tools.UI;

namespace GameFields.InformationLabels
{
    public class InformationLabelActivateData : IData
    {
        private readonly LabelActivateData _labelActivateData;
        private readonly float _timeView;

        public InformationLabelActivateData(LabelActivateData labelActivateData, float timeView = 4f)
        {
            _labelActivateData = labelActivateData;
            _timeView = timeView;
        }

        public LabelActivateData LabelActivateData => _labelActivateData;
        public float TimeView => _timeView;
    }
}