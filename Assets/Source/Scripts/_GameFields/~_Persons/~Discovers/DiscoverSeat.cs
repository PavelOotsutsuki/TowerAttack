using System.Collections.Generic;
using System.Threading;
using Cards;
using Cards.Views;
using Servers;
using Tools.Settings;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverSeat : MonoBehaviour, IDiscoverClickHandler, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private DiscoverCard _discoverCard;
        [Inject] private FightProcessDBManager _fightProcessDBManager;

        private IDiscoverable _card;
        private bool? _isPlayersAction;

        //private Movement _seatMovement;
        private IDiscoverChoiceHandler _discoverChoiceHandler;

        public void Init(IDiscoverChoiceHandler discoverChoiceHandler, float scaleFactor,
            float viewDuration, IPersonObject owner, CancellationToken fightToken)
        {
            _isPlayersAction = owner switch
            {
                IPlayerObject => true,
                IEnemyAIObject => false,
                _ => null
            };
            //_seatMovement = new Movement(_rectTransform);
            _discoverChoiceHandler = discoverChoiceHandler;
            _discoverCard.Init(OnDiscoverCardClick, this, scaleFactor, viewDuration, fightToken);
            Reset();
        }

        public void SetCard(IDiscoverable card)
        {
            _card = card;
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersAction, _card.ViewData.Number.ToString(), "VIEW", "DISCOVER");
            //DiscoverCardActivateData data = new DiscoverCardActivateData(_card.ReadOnlyRectTransform.GetSizeDelta(), _card.ViewData);
            DiscoverCardActivateData data = new DiscoverCardActivateData(GameSettings.CardSize, _card);
            _discoverCard.Activate(data);
        }

        public void StartClick()
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersAction, _card.ViewData.Number.ToString(), "CHOICE", "DISCOVER");
            _discoverCard.StartClickActions();
        }

        public void Reset()
        {
            _card = null;
            _discoverCard.Deactivate();
        }

        public void SetRectTransformValues(float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY,
            Vector2 anchorPosition)
        {
            _rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
            _rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);

            _rectTransform.anchoredPosition = anchorPosition;
        }

        //public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        //{
        //    _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        //}

        private void OnDiscoverCardClick()
        {
            _discoverChoiceHandler.OnMakeChoice(_card);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DiscoverSeat))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineDiscoverCard()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineDiscoverCard))]
        private ComponentAttachInfo DefineDiscoverCard()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _discoverCard, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}