using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class TestGameroot : MonoBehaviour
    {
        [SerializeField] private GameObject _fightRootPrefab;
        //[SerializeField] private GameObject _prefab2;
        //[SerializeField] private GameObject _prefab3;
        private DiContainer _container;
        private GameObject _currentFight;

        //private Script0 _script0;
        private bool _wasCreated = false;

        //private DiContainer _diContainer;
        //private GameRoot _fightRoot;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _container = diContainer;

            Debug.Log($"(1.1.1) Creator: Construct");

            CreateFightRoot();
        }

        private void CreateFightRoot()
        {
            StartCoroutine(SpawnPrefabsWithDelay());
        }

        private IEnumerator SpawnPrefabsWithDelay()
        {
            Debug.Log("(3.1) Creator: Starting prefab instantiation with 5-second delays...");

            yield return new WaitForSeconds(5f);



            StartFight();

            yield return new WaitForSeconds(200f);

            EndFight();
            StartFight();


            //if (_fightRoot != null)
            //{
            //Debug.Log("(3.2) Creator: Instantiating Prefab1...");
            ////GameRoot gameRoot = _fightRootFactory.Create();

            ////if (_wasCreated == false)
            ////{
            ////    gameRoot.SetActionOnDestroy(CreateFightRoot);
            ////    _wasCreated = true;
            ////}


            //Debug.Log($"(3.4) Creator: Prefab1 instantiated at {Time.time}");
            //yield return new WaitForSeconds(5f);
            //}

            //if (_prefab2 != null)
            //{
            //    Debug.Log("(3.5) Creator: Instantiating Prefab2...");
            //    GameObject instance2 = null;

            //    try
            //    {
            //        instance2 = Instantiate(_prefab2, Vector3.zero, Quaternion.identity);

            //        if (instance2 != null)
            //        {
            //            instance2.SetActive(true);
            //            Debug.Log($"(3.6)Creator: Prefab2 instantiated. Active: {instance2.activeSelf}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.LogError($"(3.65)Creator: Error instantiating Prefab2: {ex}");
            //    }

            //    Debug.Log($"(3.7) Creator: Prefab2 instantiated at {Time.time}");
            //    yield return new WaitForSeconds(5f);
            //}

            //if (_prefab3 != null)
            //{
            //    Debug.Log("(3.8) Creator: Instantiating Prefab3...");
            //    GameObject instance3 = Instantiate(_prefab3, Vector3.zero, Quaternion.identity);
            //    //instance3.SetActive(true);

            //    if (instance3 != null)
            //    {
            //        instance3.SetActive(true);
            //        //_diContainer.InjectGameObject(instance3);
            //        Debug.Log($"(3.9)Creator: Prefab3 instantiated. Active: {instance3.activeSelf}");
            //    }

            //    Debug.Log($"(3.10) Creator: Prefab3 instantiated at {Time.time}");
            //    yield return new WaitForSeconds(5f);
            //}
        }

        /////////////////////////////////////////////////////////////////////////////////

        public void StartFight()
        {
            if (_currentFight != null)
                Destroy(_currentFight);

            _currentFight = _container.InstantiatePrefab(_fightRootPrefab);
        }

        public void EndFight()
        {
            if (_currentFight != null)
            {
                Destroy(_currentFight);
                _currentFight = null;
            }
        }
    }
}