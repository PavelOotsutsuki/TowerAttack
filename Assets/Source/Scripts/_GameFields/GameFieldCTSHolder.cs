using System.Threading;
using Tools;

namespace GameFields
{
    public class GameFieldCTSHolder : CTSHolder
    {
        public GameFieldCTSHolder(CancellationTokenSource CTS) : base(CTS)
        { }
    }
}