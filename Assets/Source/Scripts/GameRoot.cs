using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
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
            card.Init(_cardDescription, _mainCamera);
        }
    }

    [ContextMenu("DefineAllComponents")]
    private void DefineAllComponents()
    {
        DefineAllCards();
        DefineCardDescription();
        DefineCamera();
    }

    [ContextMenu("DefineAllCards")]
    private void DefineAllCards()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cards);
        //DefineComponent(ref _cards);
    }

    [ContextMenu("DefineCardDescription")]
    private void DefineCardDescription()
    {
        AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InChildren);
    }

    [ContextMenu("DefineCamera")]
    private void DefineCamera()
    {
        AutomaticFillComponents.DefineComponent(this, ref _mainCamera, ComponentLocationTypes.InChildren);
    }

    //private void DefineComponent<T>(ref T target, ComponentLocationTypes componentType)
    //{
    //    string type = target.GetType().ToString();

    //    type = GetShortType(type);

    //    if (componentType == ComponentLocationTypes.InChildren)
    //    {
    //        if (GetComponentsInChildren<T>().Length < 1)
    //        {
    //            Debug.LogError($"{type} is not found");
    //        }
    //        else
    //        {
    //            if (GetComponentsInChildren<T>().Length > 1)
    //            {
    //                Debug.LogWarning($"{type} is too much! {type} length is {GetComponentsInChildren<T>().Length}");
    //            }

    //            target = GetComponentInChildren<T>();
    //        }
    //    }

    //    if (componentType == ComponentLocationTypes.InThis)
    //    {
    //        if (GetComponents<T>().Length < 1)
    //        {
    //            Debug.LogError($"{type} is not found");
    //        }
    //        else
    //        {
    //            if (GetComponents<T>().Length > 1)
    //            {
    //                Debug.LogWarning($"{type} is too much! {type} length is {GetComponents<T>().Length}");
    //            }

    //            target = GetComponent<T>();
    //        }
    //    }
    //}

    //private void DefineComponent<T>(ref T[] targets)
    //{
    //    string type = targets.GetType().ToString();
        
    //    type = GetShortType(type);

    //    if (GetComponentsInChildren<T>().Length < 1)
    //    {
    //        Debug.LogError($"{type} is not found");
    //    }

    //    targets = GetComponentsInChildren<T>();
    //}

    //private string GetShortType(string longType)
    //{
    //    string[] splitStrings = longType.Split(new char[] { '.' });
    //    int splitLenght = splitStrings.Length;
    //    return splitStrings[splitLenght - 1];
    //}

    //enum ComponentLocationTypes
    //{
    //    InChildren,
    //    InThis
    //}
}
