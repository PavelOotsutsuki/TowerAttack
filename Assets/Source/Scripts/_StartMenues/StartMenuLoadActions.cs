using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools;
using Cards.Views.BigCardViews.Capabilities;
using System;
using StartMenues.InputSettings;
using Zenject;
using Tools.Loads;
using System.Threading;
using Cysharp.Threading.Tasks;
using Servers;
using Servers.Loaders;
using Servers.DTO;

namespace StartMenues
{
    public class StartMenuLoadActions : MonoBehaviour, IActivatable, IReactivatable<StartMenuButtonsPanelRootReactivateData>
    {
        private readonly float _colorChangeDuration = 2f;

        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;
        [SerializeField] private StoneSpawner _stoneSpawner;
        //[SerializeField] private LoadText _loadText;
        [SerializeField] private StartMenu _startMenu;

        [Inject] private LoadRoot _loadRoot;
        [Inject] private DBRoot _dBRoot;
        [Inject] private AudioLoader _audioLoader;
        [Inject] private ImageLoader _imageLoader;

        private Camera _mainCamera;
        private StartMenuInputRoot _inputRoot;
        private CancellationToken _startMenuRootToken;
        //private StartMenuSavedData _startMenuSavedData;
        public IActivatable InputRoot => _inputRoot;

        public void Init(IVolume backgroundSoundConfig, IVolume foregroundSoundConfig, CardCapabilityDescription cardCapabilityDescription,
            Action<int> onPlayClick, CancellationToken startMenuRootToken)
        {
            //_startMenuSavedData = startMenuSavedData;
            _mainCamera = Camera.main;
            _startMenuRootToken = startMenuRootToken;

            _inputRoot = new StartMenuInputRoot(_startMenu);
            _loadRoot.Init();
            //_startMenu.Init(backgroundSoundConfig, foregroundSoundConfig);
            _startMenu.Init(_inputRoot, foregroundSoundConfig, backgroundSoundConfig, cardCapabilityDescription, onPlayClick, startMenuRootToken);
            _stoneSpawner.Init();
        }

        public void Activate()
        {
            Activating(_startMenuRootToken).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            // 1. Просветление экрана
            //if (_startMenuSavedData.StoneSpawnerParent == null)
            //{
            _mainCamera.backgroundColor = _startColor;

            await UniTask.WaitForSeconds(0.5f, cancellationToken: token);

            _mainCamera.DOColor(_endColor, _colorChangeDuration).SetEase(Ease.OutQuad);

            await UniTask.WaitForSeconds(_colorChangeDuration, cancellationToken: token);

            // 2. Камнепад
            CallbackHandler startCardFilesLoadCallbackHandler = new CallbackHandler();
            StartCardFilesLoad(startCardFilesLoadCallbackHandler, token).Forget();


            LoadSession loadSession = new LoadSession();
            _loadRoot.AddSession(loadSession);
            _stoneSpawner.Activate();

            await UniTask.WaitUntil(() => _stoneSpawner.IsComplete, cancellationToken: token);
            // await UniTask.WaitUntil(() => startCardFilesLoadCallbackHandler.IsComplete, cancellationToken: token);

            loadSession.Complete();

            //_startMenuSavedData.SetStoneSpawner(_stoneSpawner.StoneSpawnerParent);
            //}
            //else
            //{
            //    _stoneSpawner.Init(_stoneSpawner.StoneSpawnerParent);
            //}

            // 3. Появление меню

            _startMenu.Activate();
        }
        private async UniTask StartCardFilesLoad(CallbackHandler callbackHandler, CancellationToken token)
        {
            try
            {
                StringIntPair[] urlsData = await _dBRoot.GetStartCardSoundsUrls(token);

                foreach (StringIntPair urlData in urlsData)
                {
                    await _audioLoader.GetClipsAsync(urlData, token);
                    await _imageLoader.GetImageAsync(urlData.key, token);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка предзагрузки item-ов карт: {ex.Message}");
            }
            finally
            {
                callbackHandler.Complete();
            }
        }
        public void Reactivate(StartMenuButtonsPanelRootReactivateData reactivateDataInvoker)
        {
            _startMenu.Reactivate(reactivateDataInvoker);
        }

    }
}
