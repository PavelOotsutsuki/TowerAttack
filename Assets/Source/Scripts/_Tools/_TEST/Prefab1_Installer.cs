using UnityEngine;
using Zenject;

namespace Tools
{
    public class Prefab1_Installer// : MonoInstaller
    {
        //[SerializeField] private Script1 _script1Instance;

        //public override void InstallBindings()
        //{
        //    Debug.Log("(0.2.1) Prefab1_Installer: Installing bindings for Prefab1...");

        //    if (_script1Instance != null)
        //    {
        //        Container.Bind<Script1>().FromInstance(_script1Instance).AsSingle().NonLazy();
        //    }
        //    else
        //    {
        //        Container.Bind<Script1>().FromNewComponentOnNewGameObject().WithGameObjectName("Script1_Instance").AsSingle().NonLazy();
        //    }

        //    Container.Bind<Prefab1>().FromComponentInHierarchy().AsSingle().NonLazy();

        //    Debug.Log("(0.2.2) Prefab1_Installer: Bindings installed");
        //}
    }
}