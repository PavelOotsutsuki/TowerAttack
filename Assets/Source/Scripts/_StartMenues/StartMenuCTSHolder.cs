using System.Threading;
using Tools;

namespace StartMenues
{
    public class StartMenuCTSHolder : CTSHolder
    {
        public StartMenuCTSHolder(CancellationTokenSource CTS) : base(CTS)
        { }
    }
}