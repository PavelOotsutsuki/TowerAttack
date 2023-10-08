using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New cardEffect", menuName = "Create cardEffect", order = 51)]
public class CardEffect : Card
{
    [SerializeField] private CardCharacter _cardCharacter;

    public CardCharacter CardCharacter => _cardCharacter;
}
