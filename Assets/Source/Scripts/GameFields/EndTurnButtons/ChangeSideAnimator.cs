using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.EndTurnButtons
{
    public class ChangeSideAnimator
    {
        private ActiveView _activeView;
        private DeactiveView _deactiveView;
        private RectTransform _buttonTransform;

        public ChangeSideAnimator(ActiveView activeView, DeactiveView deactiveView, RectTransform buttonTransform)
        {
            _activeView = activeView;
            _deactiveView = deactiveView;
            _buttonTransform = buttonTransform;
        }

        public IEnumerator PlayLockButtonAnimation(float activeViewInvertDuration, float deactiveViewInvertDuration)
        {
            
        }

        public IEnumerator PlayUnlockButtonAnimation()
        {

        }
    }
}
