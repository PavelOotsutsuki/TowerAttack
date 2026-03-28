using System;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Animations.Curses
{
    [Serializable]
    internal class CardCurseAnimation : IWorkable
    {
        [SerializeField] private Graphic[] _changedGraphics;

        private CurseAnimator _curseAnimator;

        public bool? IsActive { get; private set; } = null;

        public void Init(CurseAnimator curseAnimator)
        {
            if (_changedGraphics.Length <= 0)
                throw new Exception("Ну и что ты собрался обновлять? Хоть один объект закинь");

            _curseAnimator = curseAnimator;
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _curseAnimator.AddCard(_changedGraphics);
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