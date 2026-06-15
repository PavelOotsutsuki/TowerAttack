using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.CommonAnimations
{
    public abstract class SpriteAnimation : MonoBehaviour, ICompletable, IWorkable<SpriteAnimationActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _animSprites; //Возможно, стоит заменить на массив
        [SerializeField] private float _duration;

        private Sprite _defaultView;
        private Sprite _activeView;

        private bool _isComplete;

        public float Duration => _duration;
        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _isComplete;

        public void Init()
        {
            IsActive = false;
            _isComplete = false;

            _defaultView = _animSprites[0];
            _activeView = _animSprites[_animSprites.Count - 1];

            _image.sprite = _defaultView;
        }

        public void Activate(SpriteAnimationActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _image.sprite = data.IsActiveView ? _activeView : _defaultView;

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            gameObject.SetActive(false);
        }

        public void Play(CancellationToken token)
        {
            _isComplete = false;

            SpriteAnimationActivateData data = new SpriteAnimationActivateData(false);

            Activate(data);

            StartingAnimation(token).Forget();
        }

        private async UniTask StartingAnimation(CancellationToken token)
        {
            try
            {
                TimeSpan startTime = DateTime.Now.TimeOfDay;
                //WaitForSeconds wait = new WaitForSeconds(Time.deltaTime - _duration / _animSprites.Count);

                //Debug.Log("Начало " + _animSprites.Count + "задержка " + _duration / _animSprites.Count);
                float delay = _duration / _animSprites.Count;

                int counter = 0;

                foreach (Sprite sprite in _animSprites)
                {
                    counter++;

                    _image.sprite = sprite;

                    int addFullTime = Convert.ToInt32(delay * counter * 1000);
                    int addSeconds = addFullTime / 1000;
                    int addMilliseconds = addFullTime % 1000;
                    //Debug.Log("Процесс " + counter++ + "/" + _animSprites.Count);
                    await UniTask.WaitUntil(() => DateTime.Now.TimeOfDay >= startTime.Add(new TimeSpan(0, 0, 0, addSeconds, addMilliseconds)), cancellationToken: token);
                    //yield return new WaitUntil(() => DateTime.Now.TimeOfDay >= startTime.Add(new TimeSpan(0,0,0, addSeconds, addMilliseconds)));
                    //yield return new WaitForSeconds((_duration / _animSprites.Count) - Time.deltaTime);
                }

                _isComplete = true;
                //Debug.Log("Конец " + _animSprites.Count + " . Время: " + (DateTime.Now.TimeOfDay - startTime));
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(SpriteAnimation))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineImage()
            };

            return list;
        }

        [ContextMenu(nameof(DefineImage))]
        private ComponentAttachInfo DefineImage()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _image, ComponentLocationTypes.InThis);
        }

        #endregion
    }
}