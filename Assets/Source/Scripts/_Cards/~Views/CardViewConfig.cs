using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Views
{
    [CreateAssetMenu(fileName = "CardViewConfig", menuName = "Cards/CardViewConfig", order = 51)]
    internal class CardViewConfig : ScriptableObject, ICardViewConfig
    {
        [FormerlySerializedAs("<Number>k__BackingField")]
        [SerializeField] private int _number;
        [FormerlySerializedAs("<Icon>k__BackingField")]
        [SerializeField] private Sprite _icon;
        [FormerlySerializedAs("<Description>k__BackingField")]
        [SerializeField] private string _description;
        [FormerlySerializedAs("<Feature>k__BackingField")]
        [SerializeField] private string _feature;
        [FormerlySerializedAs("<Name>k__BackingField")]
        [SerializeField] private string _name;

        public int Number => _number;
        public Sprite Icon => _icon;
        public string Description => _description;
        public string Feature => _feature;
        public string Name => _name;
    }
}