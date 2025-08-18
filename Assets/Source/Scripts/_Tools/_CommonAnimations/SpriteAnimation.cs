using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace Tools.CommonAnimations
{
    public abstract class SpriteAnimation : MonoBehaviour, IWorkable<SpriteAnimationActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _animSprites; //Возможно, стоит заменить на массив
        [SerializeField] private float _duration;

        private Sprite _defaultView;
        private Sprite _activeView;

        public float Duration => _duration;
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            IsActive = false;

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

        public void Play()
        {
            SpriteAnimationActivateData data = new SpriteAnimationActivateData(false);

            Activate(data);

            StartingAnimation().ToUniTask();
            //PlayAnimation().ToUniTask();
        }

        //private IEnumerator StartingSuccessAnimation()
        //{
        //    WaitForSeconds wait = new WaitForSeconds(_duration / _animSprites.Count * Time.deltaTime);

        //    foreach (Sprite sprite in _animSprites)
        //    {
        //        _image.sprite = sprite;
        //        yield return wait;
        //    }
        //}

        //private IEnumerator StartingAnimation()
        //{
        //    //WaitForSeconds wait = new WaitForSeconds(_duration / _animSprites.Count);
        //    TimeSpan startTime = DateTime.Now.TimeOfDay;
        //    //WaitForSeconds wait = new WaitForSeconds(_duration / _animSprites.Count * Time.deltaTime);
        //    //int counter = 0;
        //    //float fullTime = 0f;

        //    //float duration = Convert.ToSingle((DateTime.Now.TimeOfDay - startTime).TotalSeconds);

        //    while (Convert.ToSingle((DateTime.Now.TimeOfDay - startTime).TotalSeconds) < _duration)
        //    {
        //        int index = Convert.ToInt32(Convert.ToSingle((DateTime.Now.TimeOfDay - startTime).TotalSeconds) / (_duration / _animSprites.Count));

        //        if (index > 109)
        //        {
        //            index = 109;
        //        }

        //        if (index < 0)
        //        {
        //            index = 0;
        //        }

        //        _image.sprite = _animSprites[index];
        //        //Debug.Log((_image == null).ToString());
        //        //Debug.Log((_image.sprite == null).ToString());
        //        //Debug.Log(index.ToString());
        //        //Debug.Log(_animSprites[index]);
        //        Debug.Log(_duration + ": TimeSpan: " + Convert.ToSingle((DateTime.Now.TimeOfDay - startTime).TotalSeconds));
        //        yield return null;
        //    }

        //    //foreach (Sprite sprite in _animSprites)
        //    //{
        //    //    counter++;
        //    //    //float delay = _duration / _animSprites.Count * Time.deltaTime;
        //    //    //WaitForSeconds wait = new WaitForSeconds(delay);
        //    //    //fullTime += delay;
        //    //    fullTime += _duration / _animSprites.Count;
        //    //    Debug.Log(counter + ". FullTime: " + fullTime + " TimeSpan: " + (DateTime.Now.TimeOfDay - startTime).TotalSeconds);

        //    //    _image.sprite = sprite;
        //    //    //yield return wait;
        //    //}

        //    Debug.Log((DateTime.Now.TimeOfDay - startTime).TotalSeconds);
        //    yield break;
        //}


        //private IEnumerator PlayAnimation()
        //{
        //    float timePerFrame = _duration / _animSprites.Count;
        //    int currentSpriteIndex = 0;

        //    while (currentSpriteIndex < _animSprites.Count)
        //    {
        //        _image.sprite = _animSprites[currentSpriteIndex];

        //        currentSpriteIndex++;

        //        yield return new WaitForSeconds(timePerFrame);
        //    }
        //}

        private IEnumerator StartingAnimation()
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
                yield return new WaitUntil(() => DateTime.Now.TimeOfDay >= startTime.Add(new TimeSpan(0,0,0, addSeconds, addMilliseconds)));
                //yield return new WaitForSeconds((_duration / _animSprites.Count) - Time.deltaTime);
            }

            //Debug.Log("Конец " + _animSprites.Count + " . Время: " + (DateTime.Now.TimeOfDay - startTime));
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