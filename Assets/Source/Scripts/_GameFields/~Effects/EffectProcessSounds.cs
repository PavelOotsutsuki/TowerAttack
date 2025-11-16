using System;
using System.Collections.Generic;
using Cards;
using Cards.Effects;
using UnityEngine;

namespace GameFields.Effects
{
    public class EffectProcessSounds : MonoBehaviour
    {
        [Header("36: Gunner")]

        [SerializeField] private AudioClip _gunnerAttackShotSound;

        private CardSoundVolume _volume;
        private readonly Dictionary<EffectType, AudioClip[]> _effectsClips = new Dictionary<EffectType, AudioClip[]>();

        public void Init(CardSoundVolume volume)
        {
            _volume = volume;

            _effectsClips.Add(EffectType.Gunner, new AudioClip[1] { _gunnerAttackShotSound });
        }

        public void Play(EffectType effectType, int? index = null)
        {
            if (_effectsClips.ContainsKey(effectType) == false)
                throw new Exception($"Не задан данный EffectType в EffectProcessSounds!!! EffectType: {effectType}");

            AudioClip[] clips = _effectsClips[effectType];
            AudioClip clip;

            if (clips.Length <= 0)
                throw new Exception($"Не задан ни один AudioClip для EffectType: {effectType}");

            if (clips.Length > 1 && index == null)
                Debug.LogWarning($"Внимание! Не задан index для выбора AudioClip-a. Будет взят первый! Всего клипов: {clips.Length}. EffectType: {effectType}");

            if (clips.Length == 1 || index == null)
            {
                clip = clips[0];
            }
            else
            {
                if (clips.Length < index.Value)
                    throw new IndexOutOfRangeException($"Задан неверный index для выбора клипа. index: {index.Value}, всего: {clips.Length}. EffectType: {effectType} ");

                clip = clips[index.Value];
            }

            AudioSource.PlayClipAtPoint(clip, Vector3.zero, _volume.Volume);
        }
    }
}