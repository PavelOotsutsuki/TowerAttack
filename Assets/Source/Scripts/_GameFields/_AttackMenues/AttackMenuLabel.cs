using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Tools;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackMenuLabel : MonoBehaviour
    {
        private const float LifeAlpha = 1f;
        private const float EndAlpha = 0f;

        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _fontSize = 60f;
        [SerializeField] private Vector2 _startScale = Vector2.zero;
        [SerializeField] private Vector2 _endScale = new Vector2(1f, 1f);
        [SerializeField] private float _duration = 0.3f;

        public void Init()
        {
            _label.fontSize = _fontSize;

            Deactivate();
        }

        public void Activate(string message)
        {
            gameObject.SetActive(true);
            _label.text = message;

            Color endColor = new Color(_label.color.r, _label.color.g, _label.color.b, LifeAlpha);

            _label.DOColor(endColor, _duration);
            _label.transform.DOScale(_endScale, _duration);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);

            Color startColor = new Color(_label.color.r, _label.color.g, _label.color.b, EndAlpha);

            _label.color = startColor;
            _label.transform.localScale = _startScale;
        }

        //private IEnumerator Activating()
        //{
        //    //gameObject.SetActive(true);
        //    ////_label.text = message;

        //    //float startFontSize = _label.fontSize;
        //    //float fontSizeWay = (_endFontSize - startFontSize) / _duration;

        //    //for (float time = 0f; time < _duration; time += Time.deltaTime)
        //    //{
        //    //    _label.fontSize = startFontSize + fontSizeWay * time;
        //        yield return null;
        //    //}

        //    //startFontSize = _label.fontSize;
        //    //fontSizeWay = (_endFontSize - startFontSize) / _endDuration;

        //    //float startAlpha = _label.color.a;
        //    //float alphaWay = (EndAlpha - startAlpha) / _endDuration;

        //    //Color color = new(_label.color.r, _label.color.g, _label.color.b, startAlpha);

        //    //for (float time = 0f; time < _endDuration; time += Time.deltaTime)
        //    //{
        //    //    color.a = startAlpha + alphaWay * time;
        //    //    _label.color = color;
        //    //    _label.fontSize = startFontSize + fontSizeWay * time;
        //    //    yield return null;
        //    //}

        //    //gameObject.SetActive(false);
        //}

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineLabel();
        }

        [ContextMenu(nameof(DefineLabel))]
        private void DefineLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}