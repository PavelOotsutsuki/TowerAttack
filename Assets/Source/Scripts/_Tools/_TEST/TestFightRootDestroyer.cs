using System;

namespace Tools
{
    public class TestFightRootDestroyer : IDeactivatable
    {
        private readonly Action _destroyedGameObjectAction;

        public TestFightRootDestroyer(Action destroyedGameObjectAction)
        {
            _destroyedGameObjectAction = destroyedGameObjectAction;
        }

        public void Deactivate()
        {
            _destroyedGameObjectAction?.Invoke();
        }
    }
}