using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private Card _card;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _feature;

    private void Start()
    {
        _icon.sprite = _card.Icon;
        _number.sprite = _card.Number;
        _name.text = _card.Name;
        _feature.text = _card.Feature;
    }

    //[SerializeField] private Canvas _canvas;

    //private Camera _mainCamera;

    //private void Awake()
    //{
    //    _mainCamera = Camera.main;
    //}

    //private void OnEnable()
    //{

    //}
}
