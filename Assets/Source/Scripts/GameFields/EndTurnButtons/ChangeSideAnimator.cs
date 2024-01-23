using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.EndTurnButtons
{
    public class ChangeSideAnimator
    {
        private ActiveView _activeView;
        private DeactiveView _deactiveView;

        public ChangeSideAnimator(ActiveView activeView, DeactiveView deactiveView)
        {
            _activeView = activeView;
            _deactiveView = deactiveView;
        }

        public IEnumerator LockButton()
        {

        }

        public IEnumerator UnlockButton()
        {

        }
    }
}
