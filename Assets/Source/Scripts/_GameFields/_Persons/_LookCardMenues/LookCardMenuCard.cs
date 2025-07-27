using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuCard : MonoBehaviour, IWorkable<LookCardMenuCardActivateData>, IPointerExitHandler, IPointerEnterHandler, IAutomaticFillComponents
    {
        [SerializeField, Min(0f)] private float _viewDuration = 0.5f;
        [SerializeField] private LookCardMenuCardViewLogic _viewLogic;
        [SerializeField] private CardView _cardView;
        [SerializeField] private CardBlock _cardBlock;

        private string _descriptionMessage;
        private CardDescription _description;
        private LabelActivateData _labelData;

        public bool? IsActive { get; private set; } = null;

        public void Init(CardDescription cardDescription)
        {
            _description = cardDescription;
            _viewLogic.Init(_viewDuration);
            //gameObject.SetActive(true);

            Deactivate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            Block();

            gameObject.SetActive(false);
        }

        public void Activate(LookCardMenuCardActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            Block();

            _cardView.FillData(data.CardViewData);
            _descriptionMessage = data.CardViewData.Description;
            _labelData = new LabelActivateData(_descriptionMessage);

            LookCardMenuCardViewLogicData lookCardMenuCardViewLogicData = new LookCardMenuCardViewLogicData(data.CardHeight, data.CardWidth);

            _viewLogic.Show(lookCardMenuCardViewLogicData);

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

        private IEnumerator WaitingToUnblock()
        {
            //yield return new WaitForSeconds(ViewDuration);
            yield return new WaitUntil(() => _viewLogic.IsComplete);

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

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LookCardMenuCard))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDiscoverViewLogic()
            };

            return list;
        }

        [ContextMenu(nameof(DefineDiscoverViewLogic))]
        private ComponentAttachInfo DefineDiscoverViewLogic()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _viewLogic, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}