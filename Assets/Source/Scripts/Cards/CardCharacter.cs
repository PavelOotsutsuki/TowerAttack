using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class CardCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RectTransform _rectTransform;

        private Action<Vector3, Vector3, Vector3> _discardCallback;

        public void Init(AudioClip awakeSound, Action<Vector3, Vector3, Vector3> cardActive)
        {
            _audioSource.clip = awakeSound;
            _discardCallback = cardActive;
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _audioSource.Play();
        }

        public void DiscardCard(Vector3 positionDiscard, Vector3 rotationDiscard)
        {
            Vector3 startPosition = _rectTransform.position;

            _discardCallback.Invoke(positionDiscard, rotationDiscard, startPosition);
            gameObject.SetActive(false);
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


