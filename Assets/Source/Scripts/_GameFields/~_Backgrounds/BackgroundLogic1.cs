using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Backgrounds
{
    public class BackgroundLogic1 : MonoBehaviour
    {
        [SerializeField] private Sprite[] _backgrounds;

        private SmoothlyImageChanger _smoothlyImageChanger;
        private CancellationToken _fightToken;

        private readonly Queue<Sprite> _backgroundsQueue = new Queue<Sprite>();

        public void Init(SmoothlyImageChanger smoothlyImageChanger, CancellationToken fightToken)
        {
            _smoothlyImageChanger = smoothlyImageChanger;
            _fightToken = fightToken;
        }

        public void Activate()
        {
            Activating(_fightToken).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            if (_backgroundsQueue.Count == 0)
                FillQueue();

            Sprite currentBackground = _backgroundsQueue.Dequeue();
            _smoothlyImageChanger.SetImageInstantly(currentBackground);

            while (_backgroundsQueue.Count > 0)
            {
                await UniTask.WaitForSeconds(360f, cancellationToken: token);
                currentBackground = _backgroundsQueue.Dequeue();
                _smoothlyImageChanger.SetImageSmoothly(currentBackground);
            }
        }

        private void FillQueue()
        {
            foreach (Sprite sprite in _backgrounds)
            {
                _backgroundsQueue.Enqueue(sprite);
            }
        }
    }
}