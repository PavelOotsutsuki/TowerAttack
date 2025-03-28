using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.CommonAnimations
{
    public abstract class SpriteAnimation : MonoBehaviour, IWorkable, IAutomaticFillComponents
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

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

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
            Activate();

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
            WaitForSeconds wait = new WaitForSeconds(_duration / _animSprites.Count);

            foreach (Sprite sprite in _animSprites)
            {
                _image.sprite = sprite;
                yield return wait;
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