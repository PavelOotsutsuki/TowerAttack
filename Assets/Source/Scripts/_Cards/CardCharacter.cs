using UnityEngine;
using Tools.Utils.FillComponents;

namespace Cards
{
    public class CardCharacter : MonoBehaviour, ICardState
    {
        [SerializeField] private AudioSource _audioSource;

        public void Init(AudioClip awakeSound)
        {
            _audioSource.clip = awakeSound;
            transform.localPosition = Vector2.zero;
            Hide();
        }

        public void View()
        {
            AudioSource.PlayClipAtPoint(_audioSource.clip, Vector3.zero);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        //[ContextMenu("Test sound")]
        //private void TestSound()
        //{
        //    _audioSource.Play();
        //    //AudioSource.PlayClipAtPoint(_audioSource.clip, Vector3.zero);
        //}

        #region AutomaticFillComponents
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
        #endregion
    }
}


