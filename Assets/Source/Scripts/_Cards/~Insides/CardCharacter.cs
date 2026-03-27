using UnityEngine;

namespace Cards.Insides
{
    internal class CardCharacter : MonoBehaviour, ICardState
    {
        private AudioClip _awakeSound;
        private CardSoundVolume _cardSoundVolume;

        public bool? IsShown { get; private set; } = null;

        public void Init(AudioClip awakeSound, CardSoundVolume cardSoundVolume)
        {
            _awakeSound = awakeSound;
            _cardSoundVolume = cardSoundVolume;

            transform.localPosition = Vector2.zero;

            IsShown = true;
            Hide();
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            AudioSource.PlayClipAtPoint(_awakeSound, Vector3.zero, _cardSoundVolume.Volume);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            gameObject.SetActive(false);
        }
    }
}