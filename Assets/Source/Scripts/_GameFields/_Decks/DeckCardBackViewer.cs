using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Decks
{
    internal class DeckCardBackViewer : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private DeckCardBack _template;
        [SerializeField] private float _offsetX = -10f;
        [SerializeField] private float _offsetY = 0f;

        private readonly Stack<DeckCardBack> _cardBacks = new Stack<DeckCardBack>();

        private float _startPositionX;
        private float _startPositionY;

        public void Init(float startPositionX, float startPositionY)
        {
            _startPositionX = startPositionX;
            _startPositionY = startPositionY;
        }

        public void Add()
        {
            int factor = _cardBacks.Count;

            DeckCardBack deckCardBack = Instantiate(_template, _transform);
            deckCardBack.Init();

            Vector2 cardBackPosition = new Vector2(_startPositionX + factor * _offsetX, _startPositionY + factor * _offsetY);

            deckCardBack.MoveOn(cardBackPosition);

            _cardBacks.Push(deckCardBack);
        }

        public void Remove()
        {
            if (_cardBacks.Count > 0)
            {
                DeckCardBack deckCardBack = _cardBacks.Pop();
                Destroy(deckCardBack.gameObject);
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DeckCardBackViewer))]
        public List<ComponentAttachInfo> DefineAllComponents()
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