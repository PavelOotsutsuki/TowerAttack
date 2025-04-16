using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectNumberAnimator : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private ErrorSelectNumberAnimation _errorAnimation;
        [SerializeField] private SuccessSelectNumberAnimation _successAnimation;

        private SelectNumberAnimation _currentAnimation;

        public float AnimationDuration => _currentAnimation.Duration;
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _errorAnimation.Init();
            _successAnimation.Init();
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if (_currentAnimation is not null)
            {
                _currentAnimation.Activate();
            }
            else
            {
                _errorAnimation.Activate();
                _successAnimation.Activate();
            }
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _errorAnimation.Deactivate();
            _successAnimation.Deactivate();
        }

        public void PlaySuccessAnimation()
        {
            _currentAnimation = _successAnimation;

            _currentAnimation.Play();
        }

        public void PlayErrorAnimation()
        {
            _currentAnimation = _errorAnimation;

            _currentAnimation.Play();
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectNumberAnimator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineErrorSelectNumberAnimation(),
                DefineSuccessSelectNumberAnimation()
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

        #endregion
    }
}