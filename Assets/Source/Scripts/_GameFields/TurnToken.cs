using System.Threading;

namespace GameFields
{
    public class TurnToken
    {
        private CancellationTokenSource _turnCTS;

        public TurnToken()
        { }

        public void SetTokenSource(CancellationTokenSource turnCTS)
        {
            _turnCTS?.Cancel();
            _turnCTS?.Dispose();
            _turnCTS = null;

            _turnCTS = turnCTS;
        }

        public CancellationToken Token => _turnCTS.Token;
        public bool HasSource => _turnCTS != null;
    }
}