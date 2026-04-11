using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Backgrounds
{
    public class BackgroundLogic1 : MonoBehaviour
    {
        [SerializeField] private Sprite[] _backgrounds;

        private SmoothlyImageChanger _smoothlyImageChanger;
        private Queue<Sprite> _backgroundsQueue = new Queue<Sprite>();

        public void Init(SmoothlyImageChanger smoothlyImageChanger)
        {
            _smoothlyImageChanger = smoothlyImageChanger;
        }

        public void Activate()
        {
            StartCoroutine(Activating());
        }

        private IEnumerator Activating()
        {
            if (_backgroundsQueue.Count == 0)
                FillQueue();

            Sprite currentBackground = _backgroundsQueue.Dequeue();
            _smoothlyImageChanger.SetImageInstantly(currentBackground);

            while (_backgroundsQueue.Count > 0)
            {
                yield return new WaitForSeconds(360f);
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