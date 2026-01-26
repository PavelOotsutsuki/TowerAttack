using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools;
using Cards.Views.BigCardViews.Capabilities;

namespace StartMenues
{
    public class StartMenuLoadActions : MonoBehaviour
    {
        private readonly float _colorChangeDuration = 2f;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;
        [SerializeField] private StoneSpawner _stoneSpawner;
        [SerializeField] private LoadText _loadText;
        [SerializeField] private StartMenu _startMenu;

        private StartMenuSavedData _startMenuSavedData;

        public void Init(IVolume backgroundSoundConfig, IVolume foregroundSoundConfig, CardCapabilityDescription cardCapabilityDescription,
            StartMenuSavedData startMenuSavedData)
        {
            _startMenuSavedData = startMenuSavedData;

            _loadText.Init();
            //_startMenu.Init(backgroundSoundConfig, foregroundSoundConfig);
            _startMenu.Init(foregroundSoundConfig, backgroundSoundConfig, cardCapabilityDescription);
            _stoneSpawner.Init();
        }

        public void Activate()
        {
            StartCoroutine(Activating());
        }

        private IEnumerator Activating()
        {
            // 1. Просветление экрана
            if (_startMenuSavedData.StoneSpawnerParent == null)
            {
                _mainCamera.backgroundColor = _startColor;

                yield return new WaitForSeconds(0.5f);

                _mainCamera.DOColor(_endColor, _colorChangeDuration).SetEase(Ease.OutQuad);

                yield return new WaitForSeconds(_colorChangeDuration);

                // 2. Камнепад

                _loadText.Activate();
                _stoneSpawner.Activate();

                yield return new WaitUntil(() => _stoneSpawner.IsComplete);

                _loadText.Deactivate();

                _startMenuSavedData.SetStoneSpawner(_stoneSpawner.StoneSpawnerParent);
            }
            else
            {
                _stoneSpawner.Init(_stoneSpawner.StoneSpawnerParent);
            }

            // 3. Появление меню

            _startMenu.Activate();
        }
    }
}
