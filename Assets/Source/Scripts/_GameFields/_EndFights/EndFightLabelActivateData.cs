using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.UI;
using UnityEngine;

namespace GameFields.EndFights
{
    [Serializable]
    public class EndFightLabelActivateData : IData
    {
        //private readonly LabelActivateData _nascentLabelActivateData;
        //private readonly Color _textColor;

        //public EndFightLabelActivateData(LabelActivateData nascentLabelActivateData, Color textColor)
        //{
        //    _nascentLabelActivateData = nascentLabelActivateData;
        //    _textColor = textColor;
        //}

        //public LabelActivateData NascentLabelActivateData => _nascentLabelActivateData;
        //public Color TextColor => _textColor;

        [SerializeField] public string _text;
        [field: SerializeField] public Color TextColor { get; private set; }

        public LabelActivateData NascentLabelActivateData => new LabelActivateData(_text);
    }
}
