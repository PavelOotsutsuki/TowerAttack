using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Insides;
using Cards.Views;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Views.BigCardViews.CardDescriptions;
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

        //private string _descriptionMessage;
        //private CardDescription _description;
        //private BigCard _bigCard;
        private BigCardRoot _bigCardRoot;

        private BigCardRootActivateData _bigCardRootActivateData;
        //private LabelActivateData _labelData;
        private Vector2 _bigCardSize;
        //private CardViewData _currentViewData;

        public bool? IsActive { get; private set; } = null;

        public void Init(BigCardRoot bigCardRoot, CardCapabilityDescription cardCapabilityDescription)
        {
            _ROTransform = new ReadOnlyTransform(_transform);
            _bigCardSize = GameSettings.CardSize * 2f;
            _cardView.Init(cardCapabilityDescription);
            //_description = cardDescription;
            //_bigCard = bigCard;
            _bigCardRoot = bigCardRoot;

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

            _cardView.FillData(data.CardViewData);
            //_descriptionMessage = data.CardViewData.Description;
            //BigCardShowData showData = new BigCardShowData(_bigCardSize, _ROTransform, data.CardViewData);
            CardDescriptionActivateData cardDescriptionActivateData = new CardDescriptionActivateData(data.CardViewData.Description);

            _bigCardRootActivateData = new BigCardRootActivateData(null, cardDescriptionActivateData, null);

            LookCardMenuCardViewLogicData lookCardMenuCardViewLogicData = new LookCardMenuCardViewLogicData(data.CardHeight, data.CardWidth);

            _viewLogic.Show(lookCardMenuCardViewLogicData);

            WaitingToUnblock().ToUniTask();

            gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //_description.Show(_labelData);

            //BigCardShowData showData = new BigCardShowData(_bigCardSize, _ROTransform, _currentViewData);

            //_bigCard.Show(showData);

            _bigCardRoot.Activate(_bigCardRootActivateData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideAll();
        }

        private void HideAll()
        {
            _bigCardRoot.Deactivate();
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