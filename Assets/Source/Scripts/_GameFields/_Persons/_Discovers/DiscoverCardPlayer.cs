using System;
using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using Tools.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameFields.Persons.Discovers
{
    public class DiscoverCardPlayer : DiscoverCard, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private CardView _cardView;
        [SerializeField] private CardBlock _cardBlock;

        private CardCapabilityDescription _cardCapabilityDescription;

        private BigCardRoot _bigCardRoot;
        private BigCardRootActivateData _bigCardRootActivateData;

        [Inject]
        public void Construct(BigCardRoot bigCardRoot, CardCapabilityDescription cardCapabilityDescription)
        {
            _bigCardRoot = bigCardRoot;
            _cardCapabilityDescription = cardCapabilityDescription;
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            _cardView.Init(_cardCapabilityDescription);

            Block();

            gameObject.SetActive(false);
        }

        public override void Activate(DiscoverCardActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            Block();

            _cardView.FillData(data.CardViewData);
            //BigCardShowData bigCardShowData = new BigCardShowData(new Vector2(data.CardWidth, data.CardHeight), data.ReadOnlyRectTransform, data.CardViewData);
            CardDescriptionActivateData cardDescriptionActivateData = new CardDescriptionActivateData(data.CardViewData.Description);
            _bigCardRootActivateData = new BigCardRootActivateData(null, cardDescriptionActivateData, null);

            DiscoverViewLogicData discoverViewLogicData = new DiscoverViewLogicData(data.CardHeight, data.CardWidth);

            ViewLogic.Show(discoverViewLogicData);

            WaitingToUnblock().ToUniTask();

            gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _bigCardRoot.Activate(_bigCardRootActivateData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _bigCardRoot.Deactivate();
        }

        public override void StartClickActions()
        {
            ClickCallback?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _discoverClickHandler.StartClick();

            _bigCardRoot.Deactivate();
        }

        private IEnumerator WaitingToUnblock()
        {
            //yield return new WaitForSeconds(ViewDuration);
            yield return new WaitUntil(() => ViewLogic.IsComplete);

            Unblock();
        }

        private void Block()
        {
            _cardBlock.Block();
        }

        private void Unblock()
        {
            //if (gameObject.activeSelf == true && _cardBlock.IsBlock)
            //    Debug.Log("Заблочен + активен");


            _cardBlock.Unblock();
        }
    }
}