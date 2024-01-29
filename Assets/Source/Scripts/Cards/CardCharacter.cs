using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class CardCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private Action _cardActive;

        public void Init(AudioClip awakeSound, Action cardActive)
        {
            _audioSource.clip = awakeSound;
            _cardActive = cardActive;
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _audioSource.Play();
        }

        public void DiscardCard()
        {
            _cardActive.Invoke();
            gameObject.SetActive(false);
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


