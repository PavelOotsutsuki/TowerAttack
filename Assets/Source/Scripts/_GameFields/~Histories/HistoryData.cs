using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Cards.Views;
using GameFields.Persons;
using Tools;

namespace GameFields.Histories
{
    public class HistoryData : IData
    {
        private const string PlayerColor = "00A107"; // Green
        private const string EnemyColor = "FF0000"; // Red
        private const string DefaultColor = "6F5D6B"; // Gray

        private readonly string _color;
        private readonly int _turn;
        private readonly string _msg;
        private readonly List<HistoryCardData> _historyCardDatas;

        public HistoryData(IPersonObject person, string msg, HistoryCardData historyCardData, int turn = -1)
        {
            _color = person switch
            {
                IPlayerObject => PlayerColor,
                IEnemyAIObject => EnemyColor,
                _ => DefaultColor
            };

            if (turn == -1)
                turn = Fight.TurnNumber;

            _turn = turn;
            _msg = msg;
            _historyCardDatas = new List<HistoryCardData>() { historyCardData };
        }

        public int TurnForDB => _turn;
        public bool? IsPlayersAction => _color == PlayerColor ? true : _color == EnemyColor ? false : null;
        public string Msg => _msg;

        public HistoryCardData FirstData => _historyCardDatas[0];

        public bool EqualsCardViewData(HistoryData historyData)
        {
            bool isFind = false;

            foreach (HistoryCardData historyCardData in _historyCardDatas)
            {
                foreach (HistoryCardData historyCardData1 in historyData._historyCardDatas)
                {
                    if (historyCardData.EqualsCardViewData(historyCardData1))
                    {
                        isFind = true;
                        break;
                    }
                }

                if (isFind)
                    break;
            }

            return isFind;
        }

        public void AddCard(HistoryCardData historyCardData)
        {
            _historyCardDatas.Add(historyCardData);
        }

        //public IReadOnlyList<HistoryCardData> CardViewDatas => _historyCardDatas;

        public string GetActionMsg()
        {
            return $"<color=#{_color}> Ход {_turn}: {_msg} </color>";
        }

        public string GetFullMsg(bool cardAsNumber)
        {
            Func<string> nameGetter = cardAsNumber ? GetNameByNumber : GetNameByName;

            return $"<color=#{_color}> Ход {_turn}: {_msg} </color><b>{nameGetter.Invoke()}</b>";
        }

        //public string GetFullConcatMsg(bool cardAsNumber, string lastConcatData)
        //{
        //    Func<string> nameGetter = cardAsNumber ? GetNameByNumber : GetNameByName;

        //    return $"<color=#{_color}> Ход {_turn}: {_msg} <b>{lastConcatData},{nameGetter.Invoke()}</b></color>";
        //}

        private string GetNameByNumber()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < _historyCardDatas.Count; i++)
            {
                if (i != 0)
                    stringBuilder.Append(", ");

                stringBuilder.Append(_historyCardDatas[i].Number);
            }

            return stringBuilder.ToString();
        }

        private string GetNameByName()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < _historyCardDatas.Count; i++)
            {
                if (i != 0)
                    stringBuilder.Append(", ");

                stringBuilder.Append(_historyCardDatas[i].Name);
            }

            return stringBuilder.ToString();
        }


        //public string Msg => $"<color=#{_color}> Ход {_turn}: {_msg}</color>";
    }
}