using System.Threading;

namespace Tools
{
    public class CancellationTokenData : IData
    {
        private readonly CancellationToken _token;

        public CancellationTokenData(CancellationToken token)
        {
            _token = token;
        }

        public CancellationToken Token => _token;
    }
}