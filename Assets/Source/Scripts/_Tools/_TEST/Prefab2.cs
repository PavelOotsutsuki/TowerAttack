using UnityEngine;
using Zenject;

namespace Tools
{
    public class Prefab2 : MonoBehaviour
    {
        private Script2 _script2;
        private DiContainer _sceneContainer;

        public string ObjectName => GetType().ToString().Replace("Tools.", "");
        public string ScriptName => "Script2";

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

            if (_sceneContainer.HasBinding<Script2>() == false)
            {
                GameObject script2GO = new GameObject($"{ScriptName}_From_{ObjectName}");
                _script2 = script2GO.AddComponent<Script2>();

                script2GO.transform.SetParent(this.transform);

                _sceneContainer.Bind<Script2>().FromInstance(_script2).AsSingle().NonLazy();

                Debug.Log($"{ObjectName}: {ScriptName} bound to SceneContainer");
            }
            else
            {
                _script2 = _sceneContainer.Resolve<Script2>();

                Debug.Log($"{ObjectName}: {ScriptName} resolved from SceneContainer");
            }

            _sceneContainer.InjectGameObject(this.gameObject);

            Debug.Log($"{ObjectName}: Initialization complete. {ScriptName} assigned: {_script2 != null}");
        }


        [Inject]
        private void Construct(Script2 script2)
        {
            _script2 = script2;

            Debug.Log($"{ObjectName}: Construct called! {ScriptName} type: {_script2?.GetType().Name}");

            _script2.Msg();
        }

        private void Start()
        {
            Debug.Log($"(2.2) {ObjectName}: Start: {_script2 != null}");
        }
    }
}