using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumber : SelectableButton, IAttackNumber
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;
        //[SerializeField] private Animator _animator;
        [SerializeField] private Color _errorColor;
        [SerializeField] private Color _successColor;
        //[SerializeField] private Sprite _defaultView;
        //[SerializeField] private AttackNumberAnimation _attackNumberAnimation;
        [SerializeField] private AttackNumberAnimator _animator;
        //[SerializeField] private Image _anim;
        //[SerializeField] private List<Sprite> _animSprites;
        //[SerializeField] private float _duration;

        private Action<bool> _clickCallback;
        //private ConfirmableNumbers _confirmableNumbers;

        private Color? _blockColor;

        public int Number { get; private set; }

        public void Init(int number, Vector3 position, Vector2 size, Action<bool> clickCallback)
        {
            base.Init();

            Number = number;
            _blockColor = null;
            //Image.sprite = _defaultView;
            _rectTransform.sizeDelta = size;
            _rectTransform.SetLocalPositionAndRotation(position, Quaternion.identity);
            _text.text = Number.ToString();

            //_attackNumberAnimation.Init();
            _animator.Init();
            //_animator.gameObject.SetActive(false);

            _clickCallback = clickCallback;
            //_confirmableNumbers = confirmableNumbers;
        }

        //public override void Activate()
        //{
        //    base.Activate();
        //}
        //public void Activate(AttackNumberActivateData data)
        //{
        //    base.Activate();

        //    if (data.Sprite == null)
        //    {
        //        _anim.sprite = _defaultView;
        //    }
        //    else
        //    {
        //        _anim.sprite = data.Sprite;
        //    }
        //    //_animator.SetBool("IsActivate", false);
        //    //_animator.SetTrigger("Deactivate");
        //}

        public override void Activate()
        {
            base.Activate();

            if (_blockColor is not null)
            {
                SetDisableView((Color)_blockColor);
            }

            //if (data.AttackNumberAnimationData.IsActiveView)
            //{
            //    SetDisableView();
            //}

            //_attackNumberAnimation.Activate(data.AttackNumberAnimationData);
            _animator.Activate();

            //_animator.SetBool("IsActivate", false);
            //_animator.SetTrigger("Deactivate");
        }

        public override void Deactivate()
        {
            base.Deactivate();

            //_attackNumberAnimation.Deactivate();
            _animator.Deactivate();

            //_animator.SetTrigger("Deactivate");

            //Deactivating().ToUniTask();
        }

        public void SuccessChoice()
        {
            SetDisableView(_successColor);
            //_confirmableNumbers.Add(this);

            _animator.PlaySuccessAnimation();
        }

        public void ErrorChoice()
        {
            SetDisableView(_errorColor);
            //_confirmableNumbers.Add(this);

            //_attackNumberAnimation.Play();
            _animator.PlayErrorAnimation();
            //StartingAnimation().ToUniTask();
            //_animator.SetBool("IsActivate", true);
            //_animator.SetTrigger("Activate");
        }

        private void SetDisableView(Color color)
        {
            _blockColor = color;

            Image.color = color;
            CanvasGroup.blocksRaycasts = false;
        }

        //private IEnumerator StartingAnimation()
        //{
        //    WaitForSeconds wait = new WaitForSeconds(_duration / _animSprites.Count);

        //    foreach (Sprite sprite in _animSprites)
        //    {
        //        _anim.sprite = sprite;
        //        yield return wait;
        //    }
        //}

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            //_animator.Play("Cross");
            //_animator.SetBool("IsActivate", true);
            //_animator.gameObject.SetActive(true);
            _clickCallback.Invoke(true);
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            //_animator.SetBool("IsActivate", false);
            //_animator.gameObject.SetActive(false);
            _clickCallback.Invoke(false);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumber))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineText(),
                DefineAttackNumberAnimator()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineText))]
        private ComponentAttachInfo DefineText()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InThisElseChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberAnimator))]
        private ComponentAttachInfo DefineAttackNumberAnimator()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _animator, ComponentLocationTypes.InChildren);
        }

        #endregion

        //public override void OnPointerClick(PointerEventData eventData)
        //{
        //    base.OnPointerClick(eventData);

        //    _attackButton.Activate();
        //}

        //public void Unsubscribe()
        //{
        //    _button.onClick.RemoveListener(_attackButton.Activate);
        //}

        //private void Subscribe()
        //{
        //    _button.onClick.AddListener(_attackButton.Activate);
        //}
    }
}
