using UnityEngine;
using Zenject;

namespace Tools
{
    public class Prefab2_Installer// : MonoInstaller
    {
        //[SerializeField] private Script2 _script2Instance;

        //public override void InstallBindings()
        //{
        //    Debug.Log("(0.3.1) Prefab2_Installer: Installing bindings for Prefab2...");

        //    if (_script2Instance != null)
        //    {
        //        Container.Bind<Script2>().FromInstance(_script2Instance).AsSingle().NonLazy();
        //    }
        //    else
        //    {
        //        Container.Bind<Script2>().FromNewComponentOnNewGameObject().WithGameObjectName("Script2_Instance").AsSingle().NonLazy();
        //    }

        //    Container.Bind<Prefab2>().FromComponentInHierarchy().AsSingle().NonLazy();

        //    Debug.Log("(0.3.2) Prefab2_Installer: Bindings installed");
        //}
    }
}