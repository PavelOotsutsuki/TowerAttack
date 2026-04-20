using UnityEngine;
using Zenject;

namespace Tools
{
    public class Prefab3_Installer// : MonoInstaller
    {
        //[SerializeField] private Script3 _script3Instance;

        //public override void InstallBindings()
        //{
        //    Debug.Log("(0.4.1) Prefab3_Installer: Installing bindings for Prefab3...");

        //    if (_script3Instance != null)
        //    {
        //        Container.Bind<Script3>().FromInstance(_script3Instance).AsSingle().NonLazy();
        //    }
        //    else
        //    {
        //        Container.Bind<Script3>().FromNewComponentOnNewGameObject().WithGameObjectName("Script3_Instance").AsSingle().NonLazy();
        //    }

        //    Container.Bind<Prefab3>().FromComponentInHierarchy().AsSingle().NonLazy();

        //    Debug.Log("(0.4.2) Prefab3_Installer: Bindings installed");
        //}
    }
}