using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tools
{
    public class TestCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab1; 
        [SerializeField] private GameObject _prefab2; 
        [SerializeField] private GameObject _prefab3;

        //private Script0 _script0;

        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;

            Debug.Log($"(1.1.1) Creator: Container is {(_diContainer != null ? "valid" : "null")}");

            StartCoroutine(SpawnPrefabsWithDelay());
        }

        private IEnumerator SpawnPrefabsWithDelay()
        {
            Debug.Log("(3.1) Creator: Starting prefab instantiation with 5-second delays...");

            yield return new WaitForSeconds(5f);

            if (_prefab1 != null)
            {
                Debug.Log("(3.2) Creator: Instantiating Prefab1...");
                GameObject instance1 = Instantiate(_prefab1, Vector3.zero, Quaternion.identity);

                if (instance1 != null)
                {
                    instance1.SetActive(true);
                    Debug.Log($"(3.3) Creator: Prefab1 instantiated. Active: {instance1.activeSelf}");
                }

                Debug.Log($"(3.4) Creator: Prefab1 instantiated at {Time.time}");
                yield return new WaitForSeconds(5f);
            }

            if (_prefab2 != null)
            {
                Debug.Log("(3.5) Creator: Instantiating Prefab2...");
                GameObject instance2 = null;

                try
                {
                    instance2 = Instantiate(_prefab2, Vector3.zero, Quaternion.identity);

                    if (instance2 != null)
                    {
                        instance2.SetActive(true);
                        Debug.Log($"(3.6)Creator: Prefab2 instantiated. Active: {instance2.activeSelf}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"(3.65)Creator: Error instantiating Prefab2: {ex}");
                }

                Debug.Log($"(3.7) Creator: Prefab2 instantiated at {Time.time}");
                yield return new WaitForSeconds(5f);
            }

            if (_prefab3 != null)
            {
                Debug.Log("(3.8) Creator: Instantiating Prefab3...");
                GameObject instance3 = Instantiate(_prefab3, Vector3.zero, Quaternion.identity);
                //instance3.SetActive(true);

                if (instance3 != null)
                {
                    instance3.SetActive(true);
                    //_diContainer.InjectGameObject(instance3);
                    Debug.Log($"(3.9)Creator: Prefab3 instantiated. Active: {instance3.activeSelf}");
                }

                Debug.Log($"(3.10) Creator: Prefab3 instantiated at {Time.time}");
                yield return new WaitForSeconds(5f);
            }
        }
    }
}