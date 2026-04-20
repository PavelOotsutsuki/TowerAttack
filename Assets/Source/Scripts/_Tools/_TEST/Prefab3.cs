using UnityEngine;
using Zenject;

namespace Tools
{
    public class Prefab3 : MonoBehaviour
    {
        private Script3 _script3;
        private DiContainer _sceneContainer;

        public string ObjectName => GetType().ToString().Replace("Tools.", "");
        public string ScriptName => "Script3";

        private void Awake()
        {
            Debug.Log($"{ObjectName}: Awake. Active: {gameObject.activeSelf}");

            SceneContext sceneContext = FindObjectOfType<SceneContext>();

            if (sceneContext == null)
            {
                Debug.LogError($"{ObjectName}: SceneContext is not found");
                return;
            }

            _sceneContainer = sceneContext.Container;

            if (_sceneContainer == null)
            {
                Debug.LogError($"{ObjectName}: SceneContainer is null!");
                return;
            }

            Debug.Log($"{ObjectName}: SceneContainer found");

            if (_sceneContainer.HasBinding<Script3>() == false)
            {
                GameObject script3GO = new GameObject($"{ScriptName}_From_{ObjectName}");
                _script3 = script3GO.AddComponent<Script3>();

                script3GO.transform.SetParent(this.transform);

                _sceneContainer.Bind<Script3>().FromInstance(_script3).AsSingle().NonLazy();

                Debug.Log($"{ObjectName}: {ScriptName} bound to SceneContainer");
            }
            else
            {
                _script3 = _sceneContainer.Resolve<Script3>();

                Debug.Log($"{ObjectName}: {ScriptName} resolved from SceneContainer");
            }

            _sceneContainer.InjectGameObject(this.gameObject);

            Debug.Log($"{ObjectName}: Initialization complete. {ScriptName} assigned: {_script3 != null}");
        }


        [Inject]
        private void Construct(Script3 script3)
        {
            _script3 = script3;

            Debug.Log($"{ObjectName}: Construct called! {ScriptName} type: {_script3?.GetType().Name}");

            _script3.Msg();
        }

        private void Start()
        {
            Debug.Log($"(2.3) {ObjectName}: Start: {_script3 != null}");
        }
    }
}