using UnityEngine;
using Zenject;

namespace Tools
{
    public class Prefab1 : MonoBehaviour
    {
        [SerializeField] private GameObject _script1GO;

        private Script1 _script1;
        private DiContainer _sceneContainer;

        public string ObjectName => GetType().ToString().Replace("Tools.", "");
        public string ScriptName => "Script1";

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

            if (_sceneContainer.HasBinding<Script1>() == false)
            {
                //GameObject script1GO = new GameObject($"{ScriptName}_From_{ObjectName}");
                _script1 = _script1GO.GetComponent<Script1>();

                //_script1GO.transform.SetParent(this.transform);

                _sceneContainer.Bind<Script1>().FromInstance(_script1).AsSingle();//.NonLazy();

                Debug.Log($"{ObjectName}: {ScriptName} bound to SceneContainer");
            }
            else
            {
                _script1 = _sceneContainer.Resolve<Script1>();

                Debug.Log($"{ObjectName}: {ScriptName} resolved from SceneContainer");
            }

            _sceneContainer.InjectGameObject(this.gameObject);

            Debug.Log($"{ObjectName}: Initialization complete. {ScriptName} assigned: {_script1 != null}");
        }


        [Inject]
        private void Construct(Script1 script1)
        {
            _script1 = script1;

            Debug.Log($"{ObjectName}: Construct called! {ScriptName} type: {_script1?.GetType().Name}");

            _script1.Msg();
        }

        private void Start()
        {
            Debug.Log($"(2.1) {ObjectName}: Start: {_script1 != null}");
        }
    }
}