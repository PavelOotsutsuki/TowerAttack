using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Tools.Utils.Screens
{
    public class ScreenRoot
    {
        private readonly Resolution[] _resolutions;
        private readonly List<ResolutionData> _resolutionsData;
        private readonly CancellationToken _gameRootToken;

        private CancellationTokenSource _currentCTS;
        //private UniTask _waitingToUpdateScreen;
        //private UniTask _waitingToCancelWaitingToUpdateScreen;

        public ScreenRoot(CancellationToken gameRootToken)
        {
            _resolutions = Screen.resolutions;
            _gameRootToken = gameRootToken;

            _resolutionsData = new List<ResolutionData>();

            foreach (Resolution resolution in _resolutions)
            {
                _resolutionsData.Add(new ResolutionData(resolution));
            }
        }

        public Resolution CurrentResolution => Screen.currentResolution;

        public event Action OnChangeResolution;

        public void SetResolution(Resolution resolution)
        {
            if (_resolutionsData.Select(rd => rd.Resolution).Contains(resolution) == false)
            {
                throw new Exception("Ошибка задания Resolution. Такого Resolution в списке нет!");
            }

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            //CancelWaitingToUpdateScreen();

            Utils.DestroyCTS(ref _currentCTS);
            Debug.Log("SetResolution.CreateToken: " + resolution);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameRootToken);
            WaitingToUpdateScreen(resolution, _currentCTS.Token).Forget();
            Debug.Log("SetResolution.WaitingToUpdateScreen: " + resolution);

            WaitingToCancelWaitingToUpdateScreen(_gameRootToken).Forget();
            Debug.Log("SetResolution.WaitingToCancelWaitingToUpdateScreen: " + resolution);
        }

        private async UniTask WaitingToUpdateScreen(Resolution resolution, CancellationToken token)
        {
            try
            {
                await UniTask.WaitUntil(() => CurrentResolution.Equals(resolution), cancellationToken: token);
                await UniTask.WaitForSeconds(0.1f, cancellationToken: token);

                Debug.Log("Update Screen!");
                OnChangeResolution?.Invoke();
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log("Недождались смены Resolution, отмена токена");
            }
        }

        private async UniTask WaitingToCancelWaitingToUpdateScreen(CancellationToken token)
        {
            await UniTask.WaitForSeconds(5f, cancellationToken: token); // За 5 секунд не обновил - никогда не обновит

            Debug.Log("Закрываем вручную :(");
            Utils.DestroyCTS(ref _currentCTS);
        }

        //private void CancelWaitingToUpdateScreen()
        //{
        //    if (_waitingToUpdateScreen.Status == UniTaskStatus.Pending ||
        //        _waitingToCancelWaitingToUpdateScreen.Status == UniTaskStatus.Pending)
        //    {
        //        _currentToken.Cancel();
        //    }
        //}

        public string GetResolutionData(Resolution resolution)
        {
            for (int i = 0; i < _resolutionsData.Count; i++)
            {
                //if (_resolutionsData[i].Resolution.Equals(resolution))
                if (_resolutionsData[i].Resolution.Equals(resolution))
                {
                    return _resolutionsData[i].Text;
                }
            }

            throw new Exception($"Не найден Resolution по resolution: {resolution}");
        }

        public Resolution GetResolutionData(string resolutionText)
        {
            for (int i = 0; i < _resolutionsData.Count; i++)
            {
                if (_resolutionsData[i].Text == resolutionText)
                {
                    return _resolutionsData[i].Resolution;
                }
            }

            throw new Exception($"Не найден Resolution по тексту: {resolutionText}");
        }

        public IEnumerable<Resolution> Resolutions => _resolutions;
    }
}