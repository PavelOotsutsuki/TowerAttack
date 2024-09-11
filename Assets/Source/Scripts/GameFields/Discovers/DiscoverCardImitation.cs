using System;
using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    internal class DiscoverCardImitation: DiscoverCard
    {
        [SerializeField] private Color _selectedFrameColor;
        [SerializeField] private float _selectedWaitDuration = 1f;

        private Color _defaultColor;

        public override void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler)
        {
            _defaultColor = FrameImage.color;

            base.Init(clickCallback, discoverClickHandler);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Activate(float cardHeight, float cardWidth, CardViewConfig cardViewConfig = null)
        {
            FrameImage.color = _defaultColor;

            View(cardHeight, cardWidth);
        }

        public override void StartClickActions()
        {
            ClickingImitation().ToUniTask();
        }

        private IEnumerator ClickingImitation()
        {
            FrameImage.color = _selectedFrameColor;

            yield return new WaitForSeconds(_selectedWaitDuration);

            ClickCallback?.Invoke();
        }
    }
}