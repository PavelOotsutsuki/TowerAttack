using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Animations.Curses
{
    [Serializable]
    internal class CardCurseAnimation : IWorkable
    {
        [SerializeField] private Graphic[] _changedGraphics;

        private readonly Dictionary<Graphic, Color> _changedGraphicsWithColor = new Dictionary<Graphic, Color>();

        private CurseAnimator _curseAnimator;

        public bool? IsActive { get; private set; } = null;

        public void Init(CurseAnimator curseAnimator)
        {
            if (_changedGraphics.Length <= 0)
                throw new Exception("Ну и что ты собрался обновлять? Хоть один объект закинь");

            _curseAnimator = curseAnimator;

            foreach (Graphic graphic in _changedGraphics)
            {
                _changedGraphicsWithColor.Add(graphic, graphic.color);
            }
        }

        public void Reset()
        {
            foreach ((Graphic graphic, Color color) in _changedGraphicsWithColor)
            {
                graphic.color = color;
            }
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _curseAnimator.AddCard(_changedGraphicsWithColor);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _curseAnimator.RemoveCard(_changedGraphics);
        }
    }
}