using System;
using Tools;
using Tools.UI;
using UnityEngine;

namespace GameFields.EndFights
{
    public class AddedExperienceLabelActivateData : IData
    {
        private readonly Color _winColor = new Color(3/255f, 255/255f, 27/255f, 255/255f);
        private readonly Color _loseColor = new Color(255/255f, 37/255f, 1/255f, 255/255f);
        private readonly Color _drawColor = new Color(255/255f, 169/255f, 0/255f, 255/255f);

        private readonly int _addedExperience;
        private readonly Color _textColor;
        //[field: SerializeField] public Color TextColor { get; private set; }
        public AddedExperienceLabelActivateData(int addedExperience, EndFightResults result)
        {
            _addedExperience = addedExperience;

            switch (result)
            {
                case EndFightResults.PlayerWin:
                    _textColor = _winColor;
                    break;
                case EndFightResults.EnemyWin:
                    _textColor = _loseColor;
                    break;
                case EndFightResults.Draw:
                    _textColor = _drawColor;
                    break;
                default:
                    throw new Exception($"Ошибка. Неизвестный тип EndFightResults: {result}");
            }
        }

        public LabelActivateData NascentLabelActivateData => new LabelActivateData($"+{_addedExperience} EXP");
        public Color TextColor => _textColor;
    }
}