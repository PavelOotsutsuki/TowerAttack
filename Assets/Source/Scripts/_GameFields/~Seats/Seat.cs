using UnityEngine;
using Cards;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using Tools;
using System.Collections.Generic;
using Cards.Views;
using Servers;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using System.Reflection;

namespace GameFields.Seats
{
    public class Seat : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;

        private FightProcessDBManager _fightProcessDBManager;

        private Movement _seatMovement;
        private string _owner;
        private bool? _isPlayersAction;

        public Card Card { get; private set; }
        public ReadOnlyTransform ReadOnlyTransform { get; private set; }

        public void Init(FightProcessDBManager fightProcessDBManager)
        {
            _fightProcessDBManager = fightProcessDBManager;
            _seatMovement = new Movement(_transform);
            ReadOnlyTransform = new ReadOnlyTransform(_transform);
            Card = null;
        }

        public void SetOwner(string owner, bool? isPlayersAction)
        {
            _owner = owner;
            _isPlayersAction = isPlayersAction;
        }

        public void Reset()
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersAction, Card.ViewData.Number.ToString(), "REMOVE", _owner);
            Card = null;
        }

        public void SetCard(Card card, SideType sideType, float duration, CallbackHandler waitToMovement, CancellationToken token,  float scaleFactor = 1f)
        {
            Card = card;
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersAction, Card.ViewData.Number.ToString(), "SEAT", _owner);

            MoveAfterCallbackComplete(sideType, duration, scaleFactor, waitToMovement, token).Forget();
        }

        public void SetCard(Card card, SideType sideType, float duration, float scaleFactor = 1f)
        {
            Card = card;
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersAction, Card.ViewData.Number.ToString(), "SEAT", _owner);

            Move(sideType, duration, scaleFactor);
        }

        public bool IsFill() => Card != null;

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        private async UniTask MoveAfterCallbackComplete(SideType sideType, float duration, float scaleFactor, CallbackHandler waitToMovement, CancellationToken token)
        {
            try
            {
                await UniTask.WaitUntil(() => waitToMovement.IsComplete, cancellationToken: token);

                Move(sideType, duration, scaleFactor);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private void Move(SideType sideType, float duration, float scaleFactor)
        {
            Card.SetSide(sideType);
            Card.RORTransform.SetParent(_transform);
            Movement cardMovement = Card.CardMovement;
            cardMovement.MoveLocalSmoothly(Vector2.zero, Quaternion.identity.eulerAngles, duration, Card.DefaultScaleVector * scaleFactor);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(Seat))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}