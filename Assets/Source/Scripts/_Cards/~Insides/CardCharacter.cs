using Cards.Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Insides
{
    internal class CardCharacter : MonoBehaviour, ICardState
    {
        //private AudioClip _awakeSound;
        //private CardSoundVolume _cardSoundVolume;
        [SerializeField] private Image _iconImage;

        public bool? IsShown { get; private set; } = null;

        //public void Init(CardSoundConfig awakeSound, CardSoundVolume cardSoundVolume)
        public void Init(Sprite icon)
        {
            //_awakeSound = awakeSound;
            //_cardSoundVolume = cardSoundVolume;
            _iconImage.sprite = icon;
            transform.localPosition = Vector2.zero;

            IsShown = true;
            Hide();
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            //AudioSource.PlayClipAtPoint(_awakeSound, Vector3.zero, _cardSoundVolume.Volume);
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