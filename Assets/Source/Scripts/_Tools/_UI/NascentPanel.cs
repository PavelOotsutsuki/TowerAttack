using DG.Tweening;
using UnityEngine;

namespace Tools.UI
{
    public class NascentPanel : MonoBehaviour, ICompletable, IWorkable
    {
        [SerializeField] private NascentData _data;

        public bool IsComplete { get; private set; }

        public void Init()
        {
            IsComplete = true;

            Deactivate();
        }

        public void Activate()
        {
            IsComplete = false;

            transform.DOScale(_data.EndScale, _data.Duration).OnComplete(() => IsComplete = true);
        }

        public void Deactivate()
        {
            transform.localScale = _data.StartScale;
        }
    }
}