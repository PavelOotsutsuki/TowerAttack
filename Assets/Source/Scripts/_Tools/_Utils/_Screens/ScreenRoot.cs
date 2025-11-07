using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Tools.Utils.Screens
{
    public class ScreenRoot
    {
        private readonly Resolution[] _resolutions;
        private readonly List<ResolutionData> _resolutionsData;

        private CancellationTokenSource _token;
        private UniTask _waitingToUpdateScreen;

        public ScreenRoot()
        {
            _resolutions = Screen.resolutions;

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

            CancelWaitingToUpdateScreen();

            _token = new CancellationTokenSource();
            _waitingToUpdateScreen = WaitingToUpdateScreen(resolution).ToUniTask(cancellationToken: _token.Token);

            WaitingToCancelWaitingToUpdateScreen().ToUniTask();
        }

        private IEnumerator WaitingToUpdateScreen(Resolution resolution)
        {
            yield return new WaitUntil(() => CurrentResolution.Equals(resolution));
            yield return new WaitForSeconds(0.1f);

            Debug.Log("Update Screen!");
            OnChangeResolution?.Invoke();
        }

        private IEnumerator WaitingToCancelWaitingToUpdateScreen()
        {
            yield return new WaitForSeconds(5f); // За 5 секунд не обновил - никогда не обновит

            Debug.Log("Закрываем вручную :(");
            CancelWaitingToUpdateScreen();
        }

        private void CancelWaitingToUpdateScreen()
        {
            if (_waitingToUpdateScreen.Status == UniTaskStatus.Pending)
            {
                _token.Cancel();
            }
        }

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