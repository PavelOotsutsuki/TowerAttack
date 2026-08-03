using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Servers;

namespace GameFields.Histories
{
    public class HistoryRoot
    {
        private readonly List<HistoryData> _historyMsg = new List<HistoryData>();
        private readonly int _maxCountHistoryPositions = 500;

        private readonly IHistoryInputType _historyInputType;
        private readonly FightProcessDBManager _fightProcessDBManager;

        public event Action<string> OnChangedByText;
        public event Action OnChangedWithoutText;

        public HistoryRoot(IHistoryInputType historyInputType, FightProcessDBManager fightProcessDBManager)
        {
            _historyInputType = historyInputType;
            _fightProcessDBManager = fightProcessDBManager;
        }

        public string GetHistoryList(bool cardAsNumber)
        {
            if (_historyMsg.Count == 0)
                return "Пока ничего нет!";

            StringBuilder historyList = new StringBuilder();

            for (int i = _historyMsg.Count - 1; i >= 0; i--)
            {
                if (i != _historyMsg.Count - 1)
                    historyList.Append("\n--------------\n");

                historyList.Append(_historyMsg[i].GetFullMsg(cardAsNumber));
            }

            return historyList.ToString();
        }

        public void AddMsg(HistoryData historyData)
        {
            if (_historyMsg.Count > _maxCountHistoryPositions)
                throw new Exception("Слишком много msg в истории");

            // Проверяем есть ли идентичные записи
            if (IsDuplicate(historyData))
                return;

            _fightProcessDBManager.WriteFightProcessAction(historyData.TurnForDB, historyData.IsPlayersAction, historyData.FirstData.Number, historyData.Msg, null);
            // Проверяем должна ли входить в существующую
            if (IsExtra(historyData) == false)
            {
                if (_historyMsg.Count == _maxCountHistoryPositions)
                {
                    _historyMsg.Remove(_historyMsg[0]);
                }

                _historyMsg.Add(historyData);
            }

            OnChangedByText?.Invoke(GetHistoryList(_historyInputType.IsCardAsNumber));
            OnChangedWithoutText?.Invoke();
        }

        private bool IsDuplicate(HistoryData historyData)
        {
            HistoryData findedData = _historyMsg.Where(d => d.GetFullMsg(false) == historyData.GetFullMsg(false)).FirstOrDefault();

            if (findedData != null)
            {
                //Debug.Log("Нашлась копия истории!");

                if (findedData.EqualsCardViewData(historyData))
                {
                    //Debug.Log("Нашлась ПОЛНАЯ копия истории!");
                    return true;
                }
            }

            return false;
        }

        private bool IsExtra(HistoryData historyData)
        {
            HistoryData findedData = _historyMsg.Where(d => d.GetActionMsg() == historyData.GetActionMsg()).FirstOrDefault();

            if (findedData != null)
            {
                //Debug.Log("Нашлась EXTRA истории!");

                findedData.AddCard(historyData.FirstData);
                return true;
            }

            return false;
        }
    }
}