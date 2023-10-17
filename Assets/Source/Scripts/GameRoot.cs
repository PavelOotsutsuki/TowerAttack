using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private CardDescription _cardDescription;
    [SerializeField] private BigCard _bigCard;
    [SerializeField] private Card[] _cards;
    [SerializeField] private Table _table;

    private void Start()
    {
        InitAll();
    }

    private void InitAll()
    {
        InitCardDescription();
        InitBigCard();
        InitTables();
        InitCards();
    }

    private void InitCardDescription()
    {
        _cardDescription.Init();
    }

    private void InitBigCard()
    {
        _bigCard.Init();
    }

    private void InitTables()
    {
        _table.Init();
    }

    private void InitCards()
    {
        foreach(Card card in _cards)
        {
            card.Init(_cardDescription, _bigCard, _table);
        }
    }

    [ContextMenu(nameof(DefineAllComponents))]
    private void DefineAllComponents()
    {
        DefineAllCards();
        DefineCardDescription();
        DefineTable();
        DefineBigCard();
    }

    [ContextMenu(nameof(DefineAllCards))]
    private void DefineAllCards()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cards);
    }

    [ContextMenu(nameof(DefineCardDescription))]
    private void DefineCardDescription()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InChildren);
    }

    [ContextMenu(nameof(DefineTable))]
    private void DefineTable()
    {
        AutomaticFillComponents.DefineComponent(this, ref _table, ComponentLocationTypes.InChildren);
    }

    [ContextMenu(nameof(DefineBigCard))]
    private void DefineBigCard()
    {
        AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
    }
}
