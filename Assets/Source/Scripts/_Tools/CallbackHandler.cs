using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class CallbackHandler : ICompletable
    {
        private bool _isComplete;

        public CallbackHandler()
        {
            _isComplete = false;
        }

        public bool IsComplete => _isComplete;

        public void Complete()
        {
            _isComplete = true;
        }
    }
}