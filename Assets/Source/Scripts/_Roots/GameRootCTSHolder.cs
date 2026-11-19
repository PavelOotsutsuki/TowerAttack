using System.Threading;
using Tools;

namespace Roots
{
    internal class GameRootCTSHolder : CTSHolder
    {
        public GameRootCTSHolder(CancellationTokenSource CTS) : base(CTS)
        { }
    }
}