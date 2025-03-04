using System;
using Tools;
using Tools.UI;
using UnityEngine;

namespace GameFields.EndFights
{
    [Serializable]
    public class EndFightLabelActivateData : IData
    {
        [SerializeField] public string _text;
        [field: SerializeField] public Color TextColor { get; private set; }

        public LabelActivateData NascentLabelActivateData => new LabelActivateData(_text);
    }
}