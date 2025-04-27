using System;
using System.Collections.Generic;
using Tools;
using Tools.CommonAnimations;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectNumberAnimator : MonoBehaviour, IWorkable<SelectNumberAnimatorActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private SuccessSelectNumberAnimation _successAnimation;
        [SerializeField] private ErrorSelectNumberAnimation _errorAnimation;
        [SerializeField] private AcceptSelectNumberAnimation _acceptSelectNumberAnimation;

        private Dictionary<NumberAnimationType, SelectNumberAnimation> _selectNumberAnimations;
        private SelectNumberAnimation[] _allAnimations;
        private SelectNumberAnimation _currentAnimation;

        public float AnimationDuration => _currentAnimation.Duration;
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _selectNumberAnimations = new Dictionary<NumberAnimationType, SelectNumberAnimation>
            {
                {NumberAnimationType.Success, _successAnimation },
                {NumberAnimationType.Error, _errorAnimation },
                {NumberAnimationType.Choice, _acceptSelectNumberAnimation }
            };

            _allAnimations = new SelectNumberAnimation[]
            {
                _successAnimation,
                _errorAnimation,
                _acceptSelectNumberAnimation
            };

            foreach (SelectNumberAnimation animation in _allAnimations)
            {
                animation.Init();
            }
        }

        public void Activate(SelectNumberAnimatorActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if (data.NumberAnimationType == null)
                return;

            SetCurrentAnimation(data.NumberAnimationType.Value);

            SpriteAnimationActivateData animationActivateData = new SpriteAnimationActivateData(true);
            _currentAnimation.Activate(animationActivateData);
            //if (_currentAnimation is not null)
            //{
            //    _currentAnimation.Activate();
            //}
            //else
            //{
            //    _errorAnimation.Activate();
            //    _successAnimation.Activate();
            //}
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _currentAnimation?.Deactivate();

            //_errorAnimation.Deactivate();
            //_successAnimation.Deactivate();
        }

        public void PlayAnimation(NumberAnimationType numberAnimationType)
        {
            SetCurrentAnimation(numberAnimationType);

            _currentAnimation.Play();
        }

        private void SetCurrentAnimation(NumberAnimationType numberAnimationType)
        {
            if (_currentAnimation != null)
            {
                Debug.Log("Сюда не должно дойти");
                _currentAnimation.Deactivate();
            }

            if (_selectNumberAnimations.ContainsKey(numberAnimationType) == false)
                throw new NullReferenceException("Неизвестный NumberAnimationType: " + numberAnimationType);

            _currentAnimation = _selectNumberAnimations[numberAnimationType];
        }

        //public void PlaySuccessAnimation()
        //{
        //    _currentAnimation = _successAnimation;

        //    _currentAnimation.Play();
        //}

        //public void PlayErrorAnimation()
        //{
        //    _currentAnimation = _errorAnimation;

        //    _currentAnimation.Play();
        //}

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectNumberAnimator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineErrorSelectNumberAnimation(),
                DefineSuccessSelectNumberAnimation(),
                DefineAcceptSelectNumberAnimation()
            };

            return list;
        }

        [ContextMenu(nameof(DefineErrorSelectNumberAnimation))]
        private ComponentAttachInfo DefineErrorSelectNumberAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _errorAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSuccessSelectNumberAnimation))]
        private ComponentAttachInfo DefineSuccessSelectNumberAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _successAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAcceptSelectNumberAnimation))]
        private ComponentAttachInfo DefineAcceptSelectNumberAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _acceptSelectNumberAnimation, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}