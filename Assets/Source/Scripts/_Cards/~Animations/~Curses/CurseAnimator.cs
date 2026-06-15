using System;
using System.Collections.Generic;
using System.Threading;
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

        private readonly List<Graphic> _allGraphics = new List<Graphic>();
        private readonly List<Graphic> _addedGraphics = new List<Graphic>();
        private readonly List<Graphic> _removedGraphics = new List<Graphic>();

        public void Init(CancellationToken token)
        {
            _token = token;
        }

        public void AddCard(Graphic[] changedGraphics)
        {
            _addedGraphics.AddRange(changedGraphics);

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

        private void CreateSequece()
        {
            try
            {
                _sequence = DOTween.Sequence();

                if (_addedGraphics.Count > 0)
                {
                    _allGraphics.AddRange(_addedGraphics);
                    _addedGraphics.Clear();
                }

                if (_removedGraphics.Count > 0)
                {
                    foreach (Graphic element in _removedGraphics)
                    {
                        if (_allGraphics.Contains(element))
                            _allGraphics.Remove(element);
                    }

                    _removedGraphics.Clear();
                }

                foreach (Graphic graphic in _allGraphics)
                {
                    _sequence.Join(graphic.DOColor(_peakColor, _durationIn).SetEase(Ease.InOutSine));
                }

                _sequence.AppendInterval(_delayIn);

                foreach (Graphic graphic in _allGraphics)
                {
                    _sequence.Join(graphic.DOColor(graphic.color, _durationOut).SetEase(Ease.InOutSine));
                }

                _sequence.AppendInterval(_delayOut);

                _sequence.SetLoops(-1).OnStepComplete(() => OnStepComplete());
            }
            catch (Exception ex)
            {
                Debug.Log($"ERROR {nameof(CurseAnimator)}: {ex.Message}");

                _allGraphics.RemoveAll(g => g == null);

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