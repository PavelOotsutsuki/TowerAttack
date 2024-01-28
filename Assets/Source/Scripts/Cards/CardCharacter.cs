using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Cards
{
    public class CardCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private Card _me;

        public void Init(AudioClip awakeSound, Card card)
        {
            _audioSource.clip = awakeSound;
            _me = card;
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _audioSource.Play();
        }

        public void DiscardCard()
        {
            _me.Activate();
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


