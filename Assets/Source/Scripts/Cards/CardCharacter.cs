using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Cards
{
    public class CardCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource; 

        public void Init(AudioClip awakeSound)
        {
            _audioSource.clip = awakeSound;
        }

        public void Activate()
        {
            _audioSource.Play();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAudioSource();
        }

        [ContextMenu(nameof(DefineAudioSource))]
        private void DefineAudioSource()
        {
            AutomaticFillComponents.DefineComponent(this, ref _audioSource, ComponentLocationTypes.InThis);
        }
    }
}


