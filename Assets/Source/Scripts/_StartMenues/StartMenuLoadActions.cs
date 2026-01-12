using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace StartMenues
{
    public class StartMenuLoadActions : MonoBehaviour
    {
        private readonly float _colorChangeDuration = 2f;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;
        [SerializeField] private StoneSpawner _stoneSpawner;

        public void Activate()
        {
            StartCoroutine(Activating());

            _stoneSpawner.Init();
        }

        private IEnumerator Activating()
        {
            // 1. Просветление экрана
            _mainCamera.backgroundColor = _startColor;

            yield return new WaitForSeconds(0.5f);

            _mainCamera.DOColor(_endColor, _colorChangeDuration).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(_colorChangeDuration);

            // 2. Камнепад

            _stoneSpawner.Activate();

            yield return new WaitUntil(() => _stoneSpawner.IsComplete);
        }
    }
}
