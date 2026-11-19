using System.Threading;

namespace Tools.UI
{
    public class LabelActivateDataAsync : CancellationTokenData
    {
        private readonly LabelActivateData _labelActivateData;

        public LabelActivateDataAsync(LabelActivateData labelActivateData, CancellationToken token) : base(token)
        {
            _labelActivateData = labelActivateData;
        }

        public string Message => _labelActivateData.Message;
    }
}