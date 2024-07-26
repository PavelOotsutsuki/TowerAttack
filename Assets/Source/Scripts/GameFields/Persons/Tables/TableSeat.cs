using UnityEngine;
using Tools;
using Cards;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private ISeatable _character;

        internal bool IsEmpty => _character == null;

        internal void SetCardCharacter(ISeatable cardCharacter)
        {
            _character = cardCharacter;
            _character.BindParent(_rectTransform);
        }

        internal void ResetCharacter() => _character = null;

        internal bool CompareCharacters(ISeatable character) => character == _character;

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
    }
}

