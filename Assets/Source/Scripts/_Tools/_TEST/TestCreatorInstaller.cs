using UnityEngine;
using Zenject;

namespace Tools
{
    public class TestCreatorInstaller : MonoInstaller
    {
        [SerializeField] private Script0 _script0Instance;

        public override void InstallBindings()
        {
            Debug.Log("(0.1.1) CreatorInstaller: Installing bindings...");

            //if (_script0Instance != null)
            //{
            //    Container.Bind<Script0>().FromInstance(_script0Instance).AsSingle().NonLazy();
            //}
            //else
            //{
            //    Container.Bind<Script0>().FromNewComponentOnNewGameObject().WithGameObjectName("Script0_Instance").AsSingle().NonLazy();
            //}

            Container.Bind<TestCreator>().FromComponentInHierarchy().AsSingle().NonLazy();

            Debug.Log("(0.1.2) CreatorInstaller: Bindings installed successfully");
        }
    }
}