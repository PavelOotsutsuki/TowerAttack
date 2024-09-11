using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    public class PlayerDiscoverCard : DiscoverCard, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _feature;

        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;
        [SerializeField] private CanvasGroup _canvasGroup;

        public override void Hide()
        {
            Block();

            gameObject.SetActive(false);
        }

        public override void Activate(float cardHeight, float cardWidth, CardViewConfig cardViewConfig = null)
        {
            Block();

            _icon.sprite = cardViewConfig.Icon;
            _number.text = cardViewConfig.Number.ToString();
            _name.text = cardViewConfig.Name;
            _feature.text = cardViewConfig.Feature;

            View(cardHeight, cardWidth);

            WaitingToUnblock().ToUniTask();
        }

        public override void StartClickActions()
        {
            ClickCallback?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _discoverClickHandler.StartClick();
        }

        private IEnumerator WaitingToUnblock()
        {
            yield return new WaitForSeconds(GrowDuration);

            Unblock();
        }

        private void Block()
        {
            FrameImage.color = _disableFrameColor;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Unblock()
        {
            FrameImage.color = _enableFrameColor;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}
