using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using Sounds;
using Tools;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Roots
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundRoot : MonoBehaviour, IActivatable, IVolume, ISoundController, IAutomaticFillComponents
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _backgroundSounds;
        [SerializeField] private float _delay = 3f;
        [SerializeField, Range(0,1)] private float _maxVolume = 0.1f;
        [SerializeField] private float _pausingDuration = 0.5f;

        private IVolume _soundConfig;
        private CancellationToken _gameFieldToken;

        private CancellationTokenSource _processingCTS;
        private IReadOnlyList<AudioClip> _shuffleClips;

        private bool _isPaused;

        public float Percent => _soundConfig.Percent; 

        public void Init(IVolume soundConfig, CancellationToken gameFieldToken)
        {
            _soundConfig = soundConfig;
            _gameFieldToken = gameFieldToken;

            if (_backgroundSounds.Length > 0)
            {
                _shuffleClips = Utils.Shuffle(_backgroundSounds);
            }

            SetVolumePercent(Percent);
            _isPaused = false;
        }

        public void SetVolumePercent(float percent)
        {
            _soundConfig.SetVolumePercent(percent);
            _audioSource.volume = _maxVolume * Percent;
        }

        public void Activate()
        {
            //_processing = StartCoroutine(Processing());
            Utils.DestroyCTS(ref _processingCTS);
            _processingCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameFieldToken);
            Processing(_processingCTS.Token).Forget();
            //StartCoroutine(Pausing());
        }

        public void Stop()
        {
            if (_isPaused)
                return;

            Pause();
        }

        public void Continue()
        {
            if (_isPaused == false)
                return;

            Unpause();
        }

        private async UniTask Processing(CancellationToken token)
        {
            for (int i = 0; i < _shuffleClips.Count; i++)
            {
                _audioSource.clip = _shuffleClips[i];
                Play();

                await UniTask.WaitUntil(() => _audioSource.isPlaying == false && _isPaused == false, cancellationToken: token);
                await UniTask.WaitForSeconds(_delay, cancellationToken: token);
            }
        }

        //private IEnumerator Pausing()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(30f);
        //        Pause();
        //        yield return new WaitForSeconds(10f);
        //        Unpause();
        //    }
        //}

        private void Pause()
        {
            Utils.DestroyCTS(ref _processingCTS);
            _processingCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameFieldToken);

            Pausing(_processingCTS.Token).Forget();
        }

        private async UniTask Pausing(CancellationToken token)
        {
            try
            {
                float startVolume = _audioSource.volume;
                _isPaused = true;

                for (float i = 0; i < _pausingDuration; i += Time.deltaTime)
                {
                    float step = i / _pausingDuration;
                    _audioSource.volume = startVolume * (1 - step);

                    await UniTask.Yield(cancellationToken: token);
                }

                _audioSource.volume = 0;
            }
            finally
            {
                _audioSource.Pause();
                Debug.Log("Pause");
                _processingCTS = null;
            }
        }

        private void Unpause()
        {
            Utils.DestroyCTS(ref _processingCTS);
            _processingCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameFieldToken);

            Unpausing(_processingCTS.Token).Forget();
        }

        private async UniTask Unpausing(CancellationToken token)
        {
            try
            {
                float endVolume = _maxVolume * Percent;
                _isPaused = false;

                for (float i = 0; i < _pausingDuration; i += Time.deltaTime)
                {
                    float step = i / _pausingDuration;
                    _audioSource.volume = endVolume * step;

                    await UniTask.Yield(cancellationToken: token);
                }

                _audioSource.volume = endVolume;
            }
            finally
            {
                _audioSource.UnPause();
                Debug.Log("Unpause");
                _processingCTS = null;
            }
        }

        private void Play()
        {
            _audioSource.Play();
            _isPaused = false;
            Debug.Log("Play");
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SoundRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAudioSource()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAudioSource))]
        private ComponentAttachInfo DefineAudioSource()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _audioSource, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}
