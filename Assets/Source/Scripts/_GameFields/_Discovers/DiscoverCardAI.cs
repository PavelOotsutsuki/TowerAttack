using System;
using System.Collections;
using Cysharp.Threading.Tasks;
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
            if (IsActive == false)
                return;

            IsActive = false;

            gameObject.SetActive(false);
        }

        public override void Activate(DiscoverCardActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _frameImage.color = _defaultColor;

            DiscoverViewLogicData discoverViewLogicData = new DiscoverViewLogicData(data.CardHeight, data.CardWidth);

            ViewLogic.Show(discoverViewLogicData);
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