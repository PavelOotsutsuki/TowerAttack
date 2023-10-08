using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Create card", order = 51)]
public class Card : ScriptableObject
{
   // [SerializeField] private CardCharacter _cardCharacter;
    [SerializeField] private Sprite _number;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private AudioClip _awakeSound;
    [SerializeField] private string _feature;
    [SerializeField] private string _name;

    //public CardCharacter CardCharacter => _cardCharacter;
    public Sprite Number => _number;
    public Sprite Icon => _icon;
    public string Description => _description;
    public AudioClip AwakeSound => _awakeSound;
    public string Feature => _feature;
    public string Name => _name;
}
