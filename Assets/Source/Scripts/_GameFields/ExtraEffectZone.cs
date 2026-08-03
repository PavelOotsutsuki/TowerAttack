using System.Collections.Generic;
using Cards;
using GameFields.CommonAnimations;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using Cards.DependencyInterlayers;
using GameFields.CardTransits;
using GameFields.Histories;
using Cysharp.Threading.Tasks;
using System.Threading;
using Servers;

namespace GameFields.Persons.Hands
{
    public abstract class ExtraEffectZone : MonoBehaviour, IWorkable, ICompletable, IExtraEffectZone, IPlayerObject
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private InvertCardAnimationData _invertCardAnimationData;
        [Inject] private FightProcessDBManager _fightProcessDBManager;

        private ICardSeatable _seatable;
        private SignalBus _bus;
        private InvertCardAnimation _invertCardAnimation;
        private HistoryRoot _historyRoot;
        private CancellationToken _fightToken;

        public bool IsComplete { get; protected set; }

        public bool? IsActive { get; private set; } = null;

        public void Init(ICardSeatable cardSeatable, SignalBus signalBus, HistoryRoot historyRoot)
        {
            _seatable = cardSeatable;
            _bus = signalBus;
            _historyRoot = historyRoot;
            _invertCardAnimation = new InvertCardAnimation(_invertCardAnimationData);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _canvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _canvasGroup.blocksRaycasts = false;
        }

        public void StartExtraEffect(Card card)
        {
            IsComplete = false;

            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, true, card.ViewData.Number.ToString(), GetDBManagerMsg(), null);
            HistoryCardData historyCardData = new HistoryCardData(card);
            HistoryData historyData = new HistoryData(this, GetHistoryMsg(), historyCardData);
            _historyRoot.AddMsg(historyData);

            Processing(card, _fightToken).Forget();
        }

        private async UniTask Processing(Card card, CancellationToken token)
        {
            _bus.Fire(new PushStepSignalPlayer(this));

            _invertCardAnimation.Play(card, token);

            await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: token);

            _seatable.SeatCard(card);

            OnEndProcessing(token);
        }

        protected abstract string GetHistoryMsg();
        protected abstract string GetDBManagerMsg();
        protected abstract void OnEndProcessing(CancellationToken token);

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ExtraEffectZone))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}