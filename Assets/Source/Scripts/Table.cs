using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private CardSeat[] _cardSeats;

    private int[] _cardSeatsSortIndices;

    public void Init()
    {
        SetCardSeatsIndices();
    }

    //public bool TrySetCardCharacter(CardCharacter cardCharacter)
    //{

    //}
    public bool TrySetCardCharacter(CardCharacter cardCharacter)
    {
        foreach (int index in _cardSeatsSortIndices)
        {
            if (_cardSeats[index].IsEmpty())
            {
                _cardSeats[index].SetCardCharacter(cardCharacter);
                return true;
            }
        }

        return false;
    }

    private void SetCardSeatsIndices()
    {
        int countSeats = _cardSeats.Length;

        _cardSeatsSortIndices = new int[countSeats];

        for (int i = 0; i < countSeats; i++)
        {
            _cardSeatsSortIndices[i] = (countSeats + 1) / 2 + (i + 1)  / 2 * ((i + 1) % 2 * 2 - 1) - 1;
        }
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineAllCardSeats();
    }

    [ContextMenu(nameof(DefineAllCardSeats))]
    private void DefineAllCardSeats()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardSeats);
    }
}
