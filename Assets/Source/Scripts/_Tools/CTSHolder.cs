using System.Threading;

namespace Tools
{
    public abstract class CTSHolder
    {
        private readonly CancellationToken _token;

        private CancellationTokenSource _CTS;

        public CTSHolder(CancellationTokenSource CTS)
        {
            _CTS = CTS;
            _token = _CTS.Token;
        }

        public CancellationTokenSource ExtractCTS()
        {
            if (_CTS == null)
                throw new System.Exception("Ошибка извлечения токена");

            CancellationTokenSource CTS = _CTS;
            _CTS = null;

            return CTS;
        }

        public CancellationToken Token => _token;
    }
}