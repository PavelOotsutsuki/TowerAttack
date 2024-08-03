using UnityEngine;
using Tools;

namespace Cards
{
    public class CardCharacter : MonoBehaviour, ISeatable
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RectTransform _rectTransform;

        public void Init(AudioClip awakeSound)
        {
            _audioSource.clip = awakeSound;
            gameObject.SetActive(false);
        }

        public void BindParent(Transform parent)
        {
            _rectTransform.SetParent(parent);
            _rectTransform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(_audioSource.clip, Vector3.zero);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public Vector3 GetPosition()
        {
            Vector3 startPosition = new Vector2(_rectTransform.position.x, _rectTransform.position.y);

            return startPosition;
        }

        #region AutomaticFillComponents
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
        #endregion 
    }
}


