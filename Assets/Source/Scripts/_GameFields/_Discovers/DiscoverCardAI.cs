using System;
using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    internal class DiscoverCardAI : DiscoverCard
    {
        [SerializeField] private Color _selectedFrameColor;
        [SerializeField] private Image _frameImage;
        [SerializeField] private float _selectedWaitDuration = 1f;

        private Color _defaultColor;

        public override void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler)
        {
            _defaultColor = _frameImage.color;

            base.Init(clickCallback, discoverClickHandler);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public override void Activate(DiscoverCardActivateData data)
        {
            _frameImage.color = _defaultColor;

            ViewLogic.View(data.CardHeight, data.CardWidth);
            //ViewLogic.View();
        }

        public override void StartClickActions()
        {
            ClickingImitation().ToUniTask();
        }

        private IEnumerator ClickingImitation()
        {
            _frameImage.color = _selectedFrameColor;

            yield return new WaitForSeconds(_selectedWaitDuration);

            ClickCallback?.Invoke();
        }
    }
}