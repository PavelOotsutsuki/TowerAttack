using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameFields.Persons.Discovers
{
    public class DiscoverCardPlayer : DiscoverCard, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private CardView _cardView;

        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;
        [SerializeField] private CanvasGroup _canvasGroup;

        private string _descriptionMessage;
        private CardDescription _description;

        [Inject]
        public void Construct(CardDescription cardDescription)
        {
            _description = cardDescription;
        }

        public override void Hide()
        {
            Block();

            gameObject.SetActive(false);
        }

        public override void Activate(float cardHeight, float cardWidth, CardViewConfig cardViewConfig = null)
        {
            Block();

            _cardView.FillData(cardViewConfig);
            _descriptionMessage = cardViewConfig.Description;

            ViewLogic.View(cardHeight, cardWidth);

            WaitingToUnblock().ToUniTask();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _description.Show(_descriptionMessage);
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
            yield return new WaitForSeconds(ViewDuration);

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