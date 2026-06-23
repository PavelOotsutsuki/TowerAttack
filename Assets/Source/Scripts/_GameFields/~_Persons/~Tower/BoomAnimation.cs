using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tools;
using Tools.CommonAnimations;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Towers
{
    public class BoomAnimation
    {
        private readonly TowerSeat _towerSeat;
        private readonly Stone[] _stones;
        private readonly ReadOnlyRectTransform _rectTransform;
        private readonly ShakeAnimation _shakeAnimation;

        private readonly BoomAnimationData _data;

        public BoomAnimation(BoomAnimationConfig config)
        {
            _towerSeat = config.TowerSeat;
            _stones = config.Stones;
            _rectTransform = config.RectTransform;

            _data = config.Data;

            _shakeAnimation = new ShakeAnimation(config.Data.ShakeAnimationConfig);
        }

        public void Play(CancellationToken token)
        {
            BoomProcessing(token).Forget();
        }

        private async UniTask BoomProcessing(CancellationToken token)
        {
            Image targetImage = _towerSeat.Card.Background;

            float duration = _data.Duration;
            float timeInWork = 0f;

            Color startColor = targetImage.color;
            Color newCardColor = _data.NewCardColor;

            while (timeInWork < duration)
            {
                timeInWork += Time.deltaTime;

                if (timeInWork > duration)
                {
                    timeInWork = duration;
                }

                targetImage.color = Color.Lerp(startColor, newCardColor, timeInWork / duration);

                await UniTask.Yield(cancellationToken: token);
            }

            _shakeAnimation.Play(token);

            _towerSeat.Card.Kill();

            Image towerImage = _towerSeat.Image;
            towerImage.color = _data.TowerImageColor;
            Color endAshesDisappearColor = new Color(towerImage.color.r, towerImage.color.g, towerImage.color.b, 0f);

            await UniTask.WaitForSeconds(_data.DelayAfterBoomCard, cancellationToken: token);

            towerImage.DOColor(endAshesDisappearColor, _data.DurationAshesDisappear);

            foreach (Stone stone in _stones)
            {
                stone.Boom(_rectTransform.GetHeight());

                await UniTask.WaitForSeconds(_data.DelayBetweenStonesBoom, cancellationToken: token);
            }
        }
    }
}