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

        public static InformationLabelActivateData operator +(InformationLabelActivateData data1, InformationLabelActivateData data2)
        {
            return new InformationLabelActivateData(new LabelActivateData
                (data1.LabelActivateData.Message + "\n" + data2.LabelActivateData.Message),
                data1.TimeView + data2.TimeView);
        }

        public LabelActivateData LabelActivateData => _labelActivateData;
        public float TimeView => _timeView;
    }
}