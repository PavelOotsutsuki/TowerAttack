using UnityEngine;
using Tools;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private GameObject _character;

        internal bool IsEmpty => _character == null;

        internal void SetCardCharacter(GameObject cardCharacter) => _character = cardCharacter;

        internal void ResetCharacter() => _character = null;

        internal bool CompareCharacters(GameObject character) => character == _character;

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

