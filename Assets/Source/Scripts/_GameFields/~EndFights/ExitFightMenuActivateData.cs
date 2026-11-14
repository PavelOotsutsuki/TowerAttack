using Tools;

namespace GameFields.EndFights
{
    public class ExitFightMenuActivateData : IData
    {
        private readonly EndFightResults _endFightResults;

        public ExitFightMenuActivateData(EndFightResults endFightResults)
        {
            _endFightResults = endFightResults;
        }

        public bool IsWin => _endFightResults == EndFightResults.PlayerWin;
    }
}