using System.Collections;
using System.Collections.Generic;
using GameFields.EndTurnButtons;
using Sounds;
using Tools;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Roots
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundRoot : MonoBehaviour, IActivatable, IVolume, IAutomaticFillComponents
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _backgroundSounds;
        [SerializeField] private float _delay = 3f;
        [SerializeField, Range(0,1)] private float _maxVolume = 0.1f;

        private IVolume _soundConfig;

        private Coroutine _processing;
        private IReadOnlyList<AudioClip> _shuffleClips;

        private bool _isPaused;

        public float Percent => _soundConfig.Percent; 

        public void Init(IVolume soundConfig)
        {
            _soundConfig = soundConfig;

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
            _processing = StartCoroutine(Processing());
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

        private IEnumerator Processing()
        {
            for (int i = 0; i < _shuffleClips.Count; i++)
            {
                _audioSource.clip = _shuffleClips[i];
                Play();

                yield return new WaitUntil(() => _audioSource.isPlaying == false && _isPaused == false);
                yield return new WaitForSeconds(_delay);
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
            _isPaused = true;
            _audioSource.Pause();
            Debug.Log("Pause");
        }

        private void Unpause()
        {
            _audioSource.UnPause();
            _isPaused = false;
            Debug.Log("Unpause");
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
