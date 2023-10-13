using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private CardDescription _cardDescription;
    [SerializeField] private Card[] _cards;

    private void Start()
    {
        InitAll();
    }

    private void InitAll()
    {
        InitCardDescription();
        InitCards();
    }

    private void InitCardDescription()
    {
        _cardDescription.Init();
    }

    private void InitCards()
    {
        foreach(Card card in _cards)
        {
            card.Init(_cardDescription);
        }
    }

    [ContextMenu("DefineAllComponents")]
    private void DefineAllComponents()
    {
        DefineAllCards();
        DefineCardDescription();
    }

    [ContextMenu("DefineAllCards")]
    private void DefineAllCards()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cards);
    }

    [ContextMenu("DefineCardDescription")]
    private void DefineCardDescription()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InChildren);
    }
}
