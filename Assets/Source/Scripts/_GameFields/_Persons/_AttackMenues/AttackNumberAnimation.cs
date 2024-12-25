using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.AttackMenues
{
    public abstract class AttackNumberAnimation : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _animSprites; //Возможно, стоит заменить на массив
        [SerializeField] private float _duration;

        private Sprite _defaultView;
        private Sprite _activeView;

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

            //if (data.IsActiveView == false)
            //{
            //    _image.sprite = _defaultView;
            //}
            //else
            //{
            //    _image.sprite = _activeView;
            //}

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            gameObject.SetActive(false);
        }

        //public void StartSuccessChoiceAnimation()
        //{
        //    StartingSuccessAnimation().ToUniTask();
        //}

        public void Play()
        {
            Activate();

            StartingAnimation().ToUniTask();
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

        private IEnumerator StartingAnimation()
        {
            WaitForSeconds wait = new WaitForSeconds(_duration / _animSprites.Count * Time.deltaTime);

            foreach (Sprite sprite in _animSprites)
            {
                _image.sprite = sprite;
                yield return wait;
            }
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberAnimation))]
        public void DefineAllComponents()
        {
            DefineImage();
        }

        [ContextMenu(nameof(DefineImage))]
        private void DefineImage()
        {
            AutomaticFillComponents.DefineComponent(this, ref _image, ComponentLocationTypes.InThis);
        }

        #endregion
    }
}