using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class CardSpriteManager : IWorkable
    {
        [SerializeField] private CardCurseAnimation _cardCurseAnimation;

        private CardSpriteModeManager _cardSpriteModeManager;

        public bool? IsActive { get; private set; } = null;

        public void Init(CardSpriteModeManager cardSpriteModeManager, CurseAnimator curseAnimator)
        {
            _cardSpriteModeManager = cardSpriteModeManager;

            _cardCurseAnimation.Init(curseAnimator);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if (_cardSpriteModeManager.IsCurse)
                _cardCurseAnimation.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _cardCurseAnimation.Deactivate();
        }
    }
}
