using UnityEngine;
using Tools;
using System;

namespace Cards
{
    public class CardCharacter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RectTransform _rectTransform;

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

        public Card DiscardCard()
        {
            //Vector3 startPosition = Vector3.zero;
            Vector3 startPosition = new Vector2(_rectTransform.position.x - 960f, _rectTransform.position.y - 540f);
            Debug.Log("World position: " + startPosition);

            _me.DiscardCard(startPosition);
            gameObject.SetActive(false);

            return _me;
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


