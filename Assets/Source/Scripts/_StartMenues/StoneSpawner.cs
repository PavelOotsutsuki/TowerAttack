using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace StartMenues
{
    public class StoneSpawner : MonoBehaviour, ICompletable
    {
        private readonly int _countStones = 210;
        private readonly List<GameObject> _stones = new List<GameObject>();

        [SerializeField] private GameObject _stoneTemplate;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _parentForStoneSpawnerParent;
        [SerializeField] private StoneSpawnerParent _template;

        private StoneSpawnerParent _parent;
        private bool _isComplete;

        public StoneSpawnerParent StoneSpawnerParent => _parent;
        public bool IsComplete => _isComplete;

        public void Init()
        {
            _parent = Instantiate(_template, _parentForStoneSpawnerParent);

            for (int i = 0; i < _countStones; i++)
            {
                GameObject stone = Instantiate(_stoneTemplate, _parent.GetTransform());
                stone.SetActive(false);

                _stones.Add(stone);
            }
        }

        // Не используется, но может понадобится
        //public void Init(StoneSpawnerParent parent)
        //{
        //    _parent = Instantiate(parent, _parentForStoneSpawnerParent);
        //}

        public void Activate()
        {
            _isComplete = false;

            Activating(this.destroyCancellationToken).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            float y = _transform.position.y;
            float z = _transform.position.z;
            float x;
            float duration;

            for (int i = 0; i < _countStones; i++)
            {
                x = Random.Range(-8.5f, 8.5f);
                _transform.position = new Vector3(x, y, z);

                GameObject stone = _stones[i];
                stone.transform.position = _transform.position;
                stone.SetActive(true);

                SetStatic(stone, token).Forget();

                duration = Random.Range(0.025f, 0.1f);
                await UniTask.WaitForSeconds(duration, cancellationToken: token);
            }

            await UniTask.WaitForSeconds(1f, cancellationToken: token);

            _isComplete = true;
        }

        private async UniTask SetStatic(GameObject stone, CancellationToken token)
        {
            await UniTask.WaitForSeconds(3f, cancellationToken: token);

            Rigidbody2D rigidbody2D = stone.GetComponent<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

    }
}