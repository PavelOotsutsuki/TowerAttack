using System.Collections;
using System.Collections.Generic;
using Cards;
using DG.Tweening;
using GameFields.Seats;
using GameFields.Signals;
using Tools;
using Tools.CommonAnimations;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour, ICardDropPlace, ICardNumberKeeper, IReadOnlyRectTransformable, IPersonObject, IAutomaticFillComponents
    {
        private const SideType DefaultSideType = SideType.Back;
        private const bool IsCardInteraction = false;

        [SerializeField] protected TowerSeat TowerSeat;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(0f)] private float _seatDuration = 0.5f;
        [SerializeField] private Stone[] _stones;
        [SerializeField] private ShakeAnimationConfig _shakeAnimationConfig;

        private SignalBus _bus;
        private ShakeAnimation _shakeAnimation;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public bool HasFreeSeat => TowerSeat.IsFill() == false;
        public ICardNumber Card => TowerSeat.Card;

        public void Init(SignalBus bus)
        {
            _bus = bus;

            TowerSeat.Init();
            ReadOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
            _shakeAnimation = new ShakeAnimation(_shakeAnimationConfig);
        }

        //public Vector3 GetPosition() => transform.position;

        public void SeatCard(Card card)
        {
            if (HasFreeSeat)
            {
                card.SetActiveInteraction(IsCardInteraction);
                TowerSeat.SetCard(card, DefaultSideType, _seatDuration);
            }
            else
            {
                Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
            }
        }

        public void Boom()
        {
            StartCoroutine(BoomProcessing());
        }

        private IEnumerator BoomProcessing()
        {
            Image targetImage = TowerSeat.Card.Background;

            float duration = 2f;
            float timeInWork = 0f;

            Color startColor = targetImage.color;
            Color newColor = Color.red;

            while (timeInWork < duration)
            {
                timeInWork += Time.deltaTime;

                if (timeInWork > duration)
                {
                    timeInWork = duration;
                }

                targetImage.color = Color.Lerp(startColor, newColor, timeInWork / duration);

                yield return null;
            }

            _shakeAnimation.Play();
            TowerSeat.Card.gameObject.SetActive(false);
            //targetImage.color = new Color(0f, 0f, 0f, 0f);
            Image tower = TowerSeat.gameObject.GetComponent<Image>();
            tower.color = Color.black;//.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            tower.DOColor(new Color(tower.color.r, tower.color.g, tower.color.b, 0f), 1f);

            foreach (Stone stone in _stones)
            {
                //stone.Boom(_rectTransform.position.y - Mathf.Abs(_rectTransform.rect.y / 2) - 40f);
                stone.Boom(_rectTransform.position.y - _rectTransform.rect.height / 2);

                Debug.Log(_rectTransform.position.y + " : _rectTransform.position.y ");
                Debug.Log(_rectTransform.rect.y + " : _rectTransform.rect.y ");
                Debug.Log(_rectTransform.rect.x + " : _rectTransform.rect.y ");
                Debug.Log(_rectTransform.rect.height + " : _rectTransform.rect.y ");
                Debug.Log(_rectTransform.rect.width + " : _rectTransform.rect.y ");
                Debug.Log(_rectTransform.position.y - _rectTransform.rect.height / 2 + " : _rectTransform.position.y - _rectTransform.rect.y / 2");

                yield return new WaitForSeconds(0.02f);
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Tower))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerSeat(),
                DefineRectTransform(),
                DefineStones()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private ComponentAttachInfo DefineTowerSeat()
        {
           return AutomaticFillComponents.DefineComponent(this, ref TowerSeat, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineStones))]
        private ComponentAttachInfo DefineStones()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _stones);
        }
        #endregion
    }
}