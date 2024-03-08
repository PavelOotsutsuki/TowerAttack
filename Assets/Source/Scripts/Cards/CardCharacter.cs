using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class CardCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RectTransform _rectTransform;

        private CardEffect _effect;

        public void Init(AudioClip awakeSound, CardEffect effect)
        {
            _audioSource.clip = awakeSound;
            _effect = effect;
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(_audioSource.clip, Vector3.zero);
            _effect.Play();
        }

        public Vector3 DiscardCard()
        {
            Vector3 startPosition = new Vector2(_rectTransform.position.x, _rectTransform.position.y);

            gameObject.SetActive(false);

            return startPosition;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAudioSource();
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineAudioSource))]
        private void DefineAudioSource()
        {
            AutomaticFillComponents.DefineComponent(this, ref _audioSource, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
    }
}


