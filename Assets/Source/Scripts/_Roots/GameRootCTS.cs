using System.Threading;

namespace Roots
{
    internal class GameRootCTSHolder
    {
        private CancellationTokenSource _gameRootCTS;

        public GameRootCTSHolder(CancellationTokenSource gameRootCTS)
        {
            _gameRootCTS = gameRootCTS;
        }

        public CancellationTokenSource ExtractCTS()
        {
            if (_gameRootCTS == null)
                throw new System.Exception("Ошибка извлечения токена");

            CancellationTokenSource CTS = _gameRootCTS;
            _gameRootCTS = null;

            return CTS;
        }
    }
}