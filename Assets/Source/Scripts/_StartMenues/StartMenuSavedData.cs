using UnityEngine;

namespace StartMenues
{
    [CreateAssetMenu(fileName = "StartMenuSavedData", menuName = "SceneSavedData/StartMenuSavedData", order = 51)]
    public class StartMenuSavedData : ScriptableObject
    {
        [SerializeField] private StoneSpawnerParent _stoneSpawnerParent;

        public StoneSpawnerParent StoneSpawnerParent => _stoneSpawnerParent;

        public void SetStoneSpawner(StoneSpawnerParent stoneSpawnerParent)
        {
            _stoneSpawnerParent = stoneSpawnerParent;
            DontDestroyOnLoad(stoneSpawnerParent.gameObject);
        }

        public void ResetStoneSpawner()
        {
            _stoneSpawnerParent = null;
        }
    }
}