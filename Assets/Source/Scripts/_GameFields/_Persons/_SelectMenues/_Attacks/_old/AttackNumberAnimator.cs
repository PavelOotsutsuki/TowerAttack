using System.Collections.Generic;
using Tools;
using Tools.CommonAnimations;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackNumberAnimator: MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private ErrorAttackNumberAnimation _errorAnimation;
        [SerializeField] private SuccessAttackNumberAnimation _successAnimation;

        private AttackNumberAnimation _currentAnimation;

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

            SpriteAnimationActivateData data = new SpriteAnimationActivateData(true);

            if (_currentAnimation is not null)
            {
                _currentAnimation.Activate(data);
            }
            else
            {
                _errorAnimation.Activate(data);
                _successAnimation.Activate(data);
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

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberAnimator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineErrorAttackNumberAnimation(),
                DefineSuccessAttackNumberAnimation()
            };

            return list;
        }

        [ContextMenu(nameof(DefineErrorAttackNumberAnimation))]
        private ComponentAttachInfo DefineErrorAttackNumberAnimation()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _errorAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSuccessAttackNumberAnimation))]
        private ComponentAttachInfo DefineSuccessAttackNumberAnimation()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _successAnimation, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}