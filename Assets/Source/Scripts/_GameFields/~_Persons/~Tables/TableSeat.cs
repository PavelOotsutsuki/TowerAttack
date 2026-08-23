using UnityEngine;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using System.Collections.Generic;
using Zenject;
using Servers;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [Inject] private FightProcessDBManager _fightProcessDBManager;

        private bool? _isPlayerObject;
        private string _name;

        private PersonEffect _personEffect;

        internal bool IsEmpty => _personEffect == null;
        internal PersonEffect PersonEffect => _personEffect;

        //internal void SetCard(Card card)
        //{
        //    _card = card;
        //    _card.ReadOnlyRectTransform.SetParent(_rectTransform);

        //    Movement cardMovement = _card.CardMovement;

        //    cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
        //}
        internal void Init(IPersonObject owner)
        {
            switch (owner)
            {
                case IPlayerObject:
                    _isPlayerObject = true;
                    _name = nameof(TablePlayer);
                    break;
                case IEnemyAIObject:
                    _isPlayerObject = false;
                    _name = nameof(TableAI);
                    break;
                default:
                    _isPlayerObject = null;
                    _name = nameof(Table);
                    break;
            }
        }

        internal void SetCard(PersonEffect personEffect)
        {
            _personEffect = personEffect;
            //_fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayerObject, _personEffect.Card.ViewData.Number.ToString(), "PLAY", _name);
            _personEffect.Card.RORTransform.SetParent(_rectTransform);

            Movement cardMovement = _personEffect.Card.CardMovement;

            cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
        }

        internal void Reset()
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayerObject, _personEffect.Card.ViewData.Number.ToString(), "DISCARD", _name);
            _personEffect = null;
        }

        //internal bool IsCardEqual(Card card) => card == _card;

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TableSeat))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}