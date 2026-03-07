using UnityEngine;

namespace Tools
{
    public class Script3 : MonoBehaviour
    {
        public string ObjectName => GetType().ToString().Replace("Tools.", "");

        private void Awake()
        {
            Debug.Log($"{ObjectName}: Awake. Created by: {GetCreatedSource()}");
        }

        public void Msg()
        {
            Debug.Log($"{ObjectName} Msg");
        }

        private string GetCreatedSource()
        {
            if (transform.parent != null)
                return transform.parent.name;

            return transform.name;
        }
    }
}