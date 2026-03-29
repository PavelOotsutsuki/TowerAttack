using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace StartMenues
{
    public class LoadText : MonoBehaviour
    {
        private readonly float _endScale = 1.27f;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _transform;
        [SerializeField] private TMP_Text _text;

        private Sequence _currentSequence;
        //private Coroutine _currentCoroutine;

        public void Init()
        {
            _canvasGroup.alpha = 0;

            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);

            _canvasGroup.DOFade(1f, 0.5f);

            //_currentCoroutine = StartCoroutine(TextSetting());

            Sequence sequence = DOTween.Sequence().Join(_transform.DOScale(new Vector3(_endScale, _endScale, _endScale), 1f).SetLoops(-1, LoopType.Yoyo));

            _currentSequence = sequence;
        }

        public void Deactivate()
        {
            _canvasGroup.DOFade(0f, 0.5f).OnComplete(() => StopSequence());
        }

        private IEnumerator TextSetting()
        {
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    string text = "Загрузка" + new string('.', i + 1);
                    _text.text = text;
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        private void StopSequence()
        {
            if (_currentSequence != null)
            {
                if (_currentSequence.IsActive())
                {
                    _currentSequence.Kill();
                }
            }

            //if (_currentCoroutine != null)
            //{
            //    StopCoroutine(_currentCoroutine);
            //    _currentCoroutine = null;
            //}

            gameObject.SetActive(false);
        }

        //private IEnumerator Activating()
        //{

        //}
    }
}
