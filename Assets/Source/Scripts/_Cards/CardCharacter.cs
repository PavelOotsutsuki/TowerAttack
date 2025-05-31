using UnityEngine;
using Tools.Utils.FillComponents;
using System.Collections.Generic;

namespace Cards
{
    public class CardCharacter : MonoBehaviour, ICardState//, IAutomaticFillComponents
    {
        //[SerializeField] private AudioSource _audioSource;

        private AudioClip _awakeSound;

        public bool? IsShown { get; private set; } = null;

        public void Init(AudioClip awakeSound)
        {
            //_audioSource.clip = awakeSound;
            _awakeSound = awakeSound;
            transform.localPosition = Vector2.zero;

            IsShown = true;
            Hide();
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            //AudioSource.PlayClipAtPoint(_audioSource.clip, Vector3.zero);
            AudioSource.PlayClipAtPoint(_awakeSound, Vector3.zero);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            gameObject.SetActive(false);
        }

        //[ContextMenu("Test sound")]
        //private void TestSound()
        //{
        //    _audioSource.Play();
        //    //AudioSource.PlayClipAtPoint(_audioSource.clip, Vector3.zero);
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(CardCharacter))]
        //public List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineAudioSource()
        //    };

        //    return list;
        //}

        //[ContextMenu(nameof(DefineAudioSource))]
        //private ComponentAttachInfo DefineAudioSource()
        //{
        //   return AutomaticFillComponents.DefineComponent(this, ref _audioSource, ComponentLocationTypes.InThis);
        //}
        //#endregion
    }
}