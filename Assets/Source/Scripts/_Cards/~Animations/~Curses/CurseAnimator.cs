using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Animations.Curses
{
    [Serializable]
    internal class CurseAnimator
    {
        [SerializeField] private Color _peakColor = new Color(77 / 255f, 73 / 255f, 73 / 255f, 1f);
        [SerializeField] private float _durationIn = 1f;
        [SerializeField] private float _durationOut = 1f;
        [SerializeField] private float _delayIn = 0.5f;
        [SerializeField] private float _delayOut = 1f;

        private Sequence _sequence = null;
        private CancellationToken _token;

        private readonly Dictionary<Graphic, Color> _allGraphics = new Dictionary<Graphic, Color>();
        private readonly Dictionary<Graphic, Color> _addedGraphics = new Dictionary<Graphic, Color>();
        private readonly List<Graphic> _removedGraphics = new List<Graphic>();

        public void Init(CancellationToken token)
        {
            _token = token;

            LogProcess(_token).Forget();
        }

        public void AddCard(IReadOnlyDictionary<Graphic, Color> changedGraphics)
        {
            foreach ((Graphic graphic, Color color) in changedGraphics)
            {
                _addedGraphics.Add(graphic , color);
            }

            if (_sequence == null)
            {
                CreateSequece();
            }
        }

        public void RemoveCard(Graphic[] changedGraphics)
        {
            _removedGraphics.AddRange(changedGraphics);

            if (_sequence == null)
            {
                CreateSequece();
            }
        }

        private async UniTask LogProcess(CancellationToken token)
        {
            Debug.Log("Начало LogProcess-а");
            Dictionary<string, string> colors = new Dictionary<string, string>();
            Dictionary<Graphic, Color> colorsCurrent = new Dictionary<Graphic, Color>();

            try
            {
                while (true)
                {
                    await UniTask.WaitForSeconds(3.4f, cancellationToken: token);
                    colors.Clear();

                    //foreach ((Graphic g, Color c)  in _allGraphics)
                    //{
                    //    if (colors.ContainsKey(c.ToString()) == false)
                    //    {
                    //        colors.Add(c.ToString(), g.gameObject.GetInstanceID().ToString());
                    //    }
                    //    else
                    //    {
                    //        colors[c.ToString()] += $", {g.gameObject.GetInstanceID().ToString()}";
                    //    }
                    //}
                    colorsCurrent.Clear();
                    foreach (Graphic g in _allGraphics.Keys)
                    {
                        colorsCurrent.Add(g, g.color);
                    }

                    foreach ((Graphic g, Color c) in colorsCurrent)
                    {
                        if (colors.ContainsKey(c.ToString()) == false)
                        {
                            colors.Add(c.ToString(), g.gameObject.GetInstanceID().ToString());
                        }
                        else
                        {
                            colors[c.ToString()] += $", {g.gameObject.GetInstanceID().ToString()}";
                        }
                    }

                    int counter = 0;
                    foreach ((string k, string v) in colors)
                    {
                        counter++;
                        Debug.Log($"{counter}. {k} ::: {v}");
                    }
                }
            }
            catch
            {
                Debug.Log("Конец LogProcess-а");
            }
        }

        private void CreateSequece()
        {
            try
            {
                _sequence = DOTween.Sequence();

                if (_addedGraphics.Count > 0)
                {
                    foreach ((Graphic graphic, Color color) in _addedGraphics)
                    {
                        if (_allGraphics.ContainsKey(graphic) == false)
                            _allGraphics.Add(graphic, color);
                    }

                    //_allGraphics.AddRange(_addedGraphics);
                    _addedGraphics.Clear();
                }

                if (_removedGraphics.Count > 0)
                {
                    foreach (Graphic element in _removedGraphics)
                    {
                        if (_allGraphics.ContainsKey(element))
                        {
                            element.color = _allGraphics[element]; // В деактивэйте в любом случае возвращем цвет. Это чтобы никогда он не останавливался черным "посреди" анимации
                            _allGraphics.Remove(element);
                        }
                    }

                    _removedGraphics.Clear();
                }

                foreach (Graphic graphic in _allGraphics.Keys)
                {
                    graphic.color = _allGraphics[graphic]; // В начале выставляем цвет белым
                    _sequence.Join(graphic.DOColor(_peakColor, _durationIn).SetEase(Ease.InOutSine));
                }

                _sequence.AppendInterval(_delayIn);

                foreach (Graphic graphic in _allGraphics.Keys)
                {
                    _sequence.Join(graphic.DOColor(_allGraphics[graphic], _durationOut).SetEase(Ease.InOutSine));
                    //Debug.Log($"graphic = {graphic.gameObject.GetInstanceID()}. color = {_allGraphics[graphic]}");
                }

                _sequence.AppendInterval(_delayOut);

                _sequence.SetLoops(-1).OnStepComplete(() => OnStepComplete());
            }
            catch (Exception ex)
            {
                Debug.Log($"ERROR {nameof(CurseAnimator)}: {ex.Message}");

                //_allGraphics.RemoveAll(g => g == null);
                List<Graphic> forDelete = new List<Graphic>();

                foreach (Graphic graphic in _allGraphics.Keys)
                {
                    if (graphic == null)
                        forDelete.Add(graphic);

                }

                foreach (Graphic graphic in forDelete)
                {
                    _allGraphics.Remove(graphic);
                }

                _sequence.Kill();

                CreateSequece();
            }

        }

        private void OnStepComplete()
        {
            if (_addedGraphics.Count == 0 && _removedGraphics.Count == 0)
                return;

            _sequence.Kill();

            CreateSequece();
        }
    }
}