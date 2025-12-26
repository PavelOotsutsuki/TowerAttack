using System.Collections.Generic;
using Cards.Views;
using Cards.Views.BigCardViews.CardDescriptions;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards.Insides
{
    internal class CardCharacter : MonoBehaviour, ICardState
    {
        //private AudioClip _awakeSound;
        //private CardSoundVolume _cardSoundVolume;
        [SerializeField] private Image _iconImage;
        [SerializeField] private CardFeatureHelper _cardFeatureHelper;

        public bool? IsShown { get; private set; } = null;

        //public void Init(CardSoundConfig awakeSound, CardSoundVolume cardSoundVolume)
        public void Init(Sprite icon, CardDescription cardDescription, IFeatureWatcher featureWatcher)
        {
            //_awakeSound = awakeSound;
            //_cardSoundVolume = cardSoundVolume;
            _iconImage.sprite = icon;
            transform.localPosition = Vector2.zero;
            _cardFeatureHelper.Init(cardDescription, () => featureWatcher.Feature);
            //_UIHelper.Init(UIHelperDescription, () => featureWatcher.Feature);

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

            _cardFeatureHelper.OnPointerExit(new PointerEventData(EventSystem.current));
            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardCharacter))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCardFeatureHelper()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCardFeatureHelper))]
        private ComponentAttachInfo DefineCardFeatureHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFeatureHelper, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}