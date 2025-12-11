using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanel : MonoBehaviour, ICompletable, IWorkable<LookCardMenuSeatPanelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private LookCardMenuSeat _seatTemplate;
        [SerializeField] private LookCardMenuSeatContainer _seatContainer;
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private float _maxCardSizeScale = 3f; // Высчитываем вручную размер скейла. Берем в расчетах по максимуму
                                                              // Если он слишком высок, берем это значение
        [SerializeField] private float _maxIndentStone = 60f; 
        [SerializeField] private float _indentArrowX = 100f;
        [SerializeField] private float _extraIndentX = 10f;
        [SerializeField] private float _extraIndentY = 25f;
        [SerializeField] private float _betweenCardsIndentFactor = 4f; // Во сколько раз расстояние между картами меньше размера карт

        private readonly int _maxSeats = 3;
        private readonly Vector2 _defaultCardSize = GameSettings.CardSize;

        private LookCardMenuSeat[] _seats;
        private Card[] _cards;

        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _isComplete;

        ReadOnlyRectTransform _ROTransform;

        private Vector2 _cardSize;
        private float _betweenCardsIndent;

        public int MaxSeats => _maxSeats;

        public void Init(BigCardRoot bigCardRoot, CardCapabilityDescription cardCapabilityDescription)
        {
            _ROTransform = new ReadOnlyRectTransform(_rectTransform);

            // Отступ от края по X до карты = половина max размера камня + width стрелки + доп indent для видимости непрямого смыкания
            float indentX = _maxIndentStone / 2f + _indentArrowX + _extraIndentX;
            // Отступ от края по Y до карты = половина max размера камня + доп indent для видимости непрямого смыкания
            float indentY = _maxIndentStone / 2f + _extraIndentY;
            // Коэффициент высоты/ширины карты, дабы потом правильно выбрать размеры
            float cardSizeFactor = _defaultCardSize.x / _defaultCardSize.y;
            // Рабочая область для размещения карт по ширине
            float thisWorkWidth = _ROTransform.GetWidth() - indentX * 2f;
            // Рабочая область для размещения карт по высоте
            float thisWorkHeight = _ROTransform.GetHeight() - indentY * 2f;

            float maxHeightCard = thisWorkHeight;
            // Берем за отступы между картами 1/4 размера карты. Получается n карт + (n-1) отступов
            float maxWeightCard = thisWorkWidth / (_maxSeats + ((_maxSeats - 1) / _betweenCardsIndentFactor));
            // Считаем сколько будет width если возьмем по максимуму от высоты
            float wightByMaxHeightCard = maxHeightCard * cardSizeFactor;
            // Считаем сколько будет height если возьмем по максимуму от ширины
            float heightByMaxWeightCard = maxWeightCard / cardSizeFactor;

            // Берем наименьшее, иначе не влезет
            if (heightByMaxWeightCard < maxHeightCard)
            {
                _cardSize = new Vector2(maxWeightCard, heightByMaxWeightCard);
            }
            else
            {
                _cardSize = new Vector2(wightByMaxHeightCard, maxHeightCard);
            }

            // Если наименьший слишком большой, уменьшаем до максимально большого
            if (_cardSize.x > _defaultCardSize.x * _maxCardSizeScale)
                _cardSize = _defaultCardSize * _maxCardSizeScale;

            // Заранее высчитваем indent между картами
            _betweenCardsIndent = _cardSize.x / _betweenCardsIndentFactor;

            //_lastIndex = -1;

            _seats = new LookCardMenuSeat[_maxSeats];

            for (int i = 0; i < _maxSeats; i++)
            {
                LookCardMenuSeat seat = Instantiate(_seatTemplate, _seatContainer.GetTransform());
                _seats[i] = seat;
                seat.Init(bigCardRoot, cardCapabilityDescription);
            }

            _isComplete = true;
        }

        public void Activate(LookCardMenuSeatPanelActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;
            _isComplete = false;
            gameObject.SetActive(true);

            Card[] cards = data.Cards.ToArray();
            //_lastIndex = -1;

            int countCards = cards.Length;

            if (countCards > _maxSeats)
                countCards = _maxSeats;

            _cards = new Card[countCards];

            for (int i = 0; i < countCards; i++)
            {
                _cards[i] = cards[i];
            }

            SeatCards();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            gameObject.SetActive(false);

            for (int i = 0; i < _seats.Length; i++)
            {
                _seats[i].Reset();
            }

            _isComplete = true;
        }

        private void SeatCards()
        {
            int countAll = _cards.Length;

            float startPositionX = (_betweenCardsIndent + _cardSize.x) * ((countAll - 1) / 2f) * (-1f);
            float step = _betweenCardsIndent + _cardSize.x;
            float currentPositionX;

            for (int i = 0; i < countAll; i++)
            {
                currentPositionX = startPositionX + step * i;

                Vector2 position = new Vector2(currentPositionX, 0f);

                _seats[i].SetLocalPositionValues(position, Quaternion.identity.eulerAngles);
                _seats[i].SetCard(_cards[i], _cardSize);
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LookCardMenuSeatPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLookCardMenuSeatContainer(),
                DefineRectTransform(),
            };

            return list;
        }

        [ContextMenu(nameof(DefineLookCardMenuSeatContainer))]
        private ComponentAttachInfo DefineLookCardMenuSeatContainer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _seatContainer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}