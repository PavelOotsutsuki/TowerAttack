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

        private string _descriptionMessage;
        private CardDescription _description;
        private LabelActivateData _labelData;

        [Inject]
        public void Construct(CardDescription cardDescription)
        {
            _description = cardDescription;
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

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
            _descriptionMessage = data.CardViewData.Description;
            _labelData = new LabelActivateData(_descriptionMessage);

            DiscoverViewLogicData discoverViewLogicData = new DiscoverViewLogicData(data.CardHeight, data.CardWidth);

            ViewLogic.Show(discoverViewLogicData);

            WaitingToUnblock().ToUniTask();

            gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _description.Show(_labelData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _description.Hide();
        }

        public override void StartClickActions()
        {
            ClickCallback?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _discoverClickHandler.StartClick();
            _description.Hide();
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