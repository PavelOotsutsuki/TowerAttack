using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberAnimator: MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private ErrorAttackNumberAnimation _errorAnimation;
        [SerializeField] private SuccessAttackNumberAnimation _successAnimation;

        private AttackNumberAnimation _currentAnimation;

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

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberAnimator))]
        public void DefineAllComponents()
        {
            DefineErrorAttackNumberAnimation();
            DefineSuccessAttackNumberAnimation();
        }

        [ContextMenu(nameof(DefineErrorAttackNumberAnimation))]
        private void DefineErrorAttackNumberAnimation()
        {
            AutomaticFillComponents.DefineComponent(this, ref _errorAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSuccessAttackNumberAnimation))]
        private void DefineSuccessAttackNumberAnimation()
        {
            AutomaticFillComponents.DefineComponent(this, ref _successAnimation, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}