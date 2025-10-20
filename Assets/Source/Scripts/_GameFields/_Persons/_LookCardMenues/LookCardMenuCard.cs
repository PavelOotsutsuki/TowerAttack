using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Settings;
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
        [SerializeField] private Transform _transform;
        [SerializeField] private CardView _cardView;
        [SerializeField] private CardBlock _cardBlock;

        private ReadOnlyTransform _ROTransform;

        private string _descriptionMessage;
        private CardDescription _description;
        private BigCard _bigCard;

        private LabelActivateData _labelData;
        private Vector2 _bigCardSize;
        private CardViewData _currentViewData;

        public bool? IsActive { get; private set; } = null;

        public void Init(CardDescription cardDescription, BigCard bigCard)
        {
            _ROTransform = new ReadOnlyTransform(_transform);
            _bigCardSize = GameSettings.CardSize * 2f;

            _description = cardDescription;
            _bigCard = bigCard;

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
            HideAll();

            gameObject.SetActive(false);
        }

        public void Activate(LookCardMenuCardActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            Block();

            _currentViewData = data.CardViewData;
            _cardView.FillData(_currentViewData);
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

            BigCardShowData showData = new BigCardShowData(_bigCardSize, _ROTransform, _currentViewData);

            _bigCard.Show(showData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideAll();
        }

        private void HideAll()
        {
            _description.Hide();
            _bigCard.Hide();
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
                DefineDiscoverViewLogic(),
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineDiscoverViewLogic))]
        private ComponentAttachInfo DefineDiscoverViewLogic()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _viewLogic, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}