using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [Serializable]
    public class AttackNumberAnimator: IWorkable<AttackNumberAnimationActivateData> 
    {
        [SerializeField] private AttackNumberAnimation _errorAnimation;
        [SerializeField] private AttackNumberAnimation _successAnimation;

        private AttackNumberAnimation _currentAnimation;

        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _errorAnimation.Init();
            _successAnimation.Init();
        }

        public void Activate(AttackNumberAnimationActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

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
    }
}