using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanel : MonoBehaviour, ICompletable, IWorkable<LookCardMenuSeatPanelActivateData>
    {
        //[SerializeField] private LookCardMenuSeat[] _seats;
        [SerializeField] private LookCardMenuSeat _seatTemplate;
        [SerializeField] private LookCardMenuSeatContainer _seatContainer;
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private float _maxCardSizeScale = 2f;
        [SerializeField] private float _minCardSizeScale = 1.5f;
        //[SerializeField] private float _offset = 400f;
        //[SerializeField] private float _positionY = 0f;

        private LookCardMenuSeat[] _seats;
        private Card[] _cards;

        private Vector2 _defaultCardSize;
        //private int _maxCardsInRow;
        //private int _maxCardsInColumn;

        private bool _isComplete;

        //public int MaxSeats => _seats.Length;
        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _isComplete;

        //[field: SerializeField] public float NumberWidht { get; private set; } = 100f;
        //[field: SerializeField] public float NumberHeight { get; private set; } = 100f;
        //[field: SerializeField] public float Indent { get; private set; } = 50f;


        //private int _columnsCount;
        //private int _rowsCount;
        //private int _lastRowColumnsCount;
        //private float _columnsIndent;
        //private float _rowsIndent;
        //private float _maxHeight;
        //private float _maxWidth;

        //public int MaxCount => _cards.Count > _maxCardsInRow * _maxCardsInColumn ? _maxCardsInRow * _maxCardsInColumn : _cards.Count;
        //public int ColumnsCount => _columnsCount > _maxCardsInColumn ? _maxCardsInColumn : _columnsCount;
        //public int RowsCount => _rowsCount > _maxCardsInRow ? _maxCardsInRow : _rowsCount;
        ReadOnlyRectTransform _ROTransform;

        private float _thisWidth; // width поля 
        private float _thisHeight; // height поля 

        private Vector2 _minSeatSize; // min размер карты
        private int _maxSeats; // max кол-во seat-ов, которое поместится в зону
        private int _maxSeatsInWidth; // max кол-во seat-ов, которое поместится в зоне по width
        private int _maxSeatsInHeight; // max кол-во seat-ов, которое поместится в зоне по height

        private Vector2 _maxSeatSize; // max размер карты
        private int _maxSeatsInWidthByMaxSeatSize; // max кол-во seat-ов больших карт, которое поместится в зоне по width

        private int _lastIndex; // Последний index seat-ов

        public int MaxSeats => _maxSeats;

        public void Init(CardDescription cardDescription, BigCard bigCard)
        {
            _ROTransform = new ReadOnlyRectTransform(_rectTransform);

            //Debug.Log("Convert.ToInt32(0.9f): " + Convert.ToInt32(0.9f));
            //Debug.Log("Convert.ToInt32(1.1f): " + Convert.ToInt32(1.1f));
            //Debug.Log("Convert.ToInt32(1.5f): " + Convert.ToInt32(1.5f));
            //Debug.Log("Convert.ToInt32(1.9f): " + Convert.ToInt32(1.9f));

            _thisWidth = _ROTransform.GetWidth();
            _thisHeight = _ROTransform.GetHeight();

            _defaultCardSize = GameSettings.CardSize;
            _minSeatSize = _defaultCardSize * _minCardSizeScale;

            // Тк размер поля должен вмещать в себя не только карты но и indent-ы между картами и на краях, берем что indent = 0.5 * карты
            // Итого: из-за indent-а между картами + 1-го indent-a скраю, надо делить на size карты + size карты / 2f = 1.5f * size карты
            // при этом, надо вычесть из поля 1 крайний indent
            // Пример: карта 200, поле 400, помещается ровно 1 карта тк + 2 крайних indent-a.
            // Пример2: карта 200, поле 700, помещается ровно 2 карты тк + 2 крайних и 1 между indent
            _maxSeatsInWidth = (int)((_thisWidth - _minSeatSize.x / 2f) / (_minSeatSize.x * 1.5f));
            _maxSeatsInHeight = (int)((_thisHeight - _minSeatSize.y / 2f) / (_minSeatSize.y * 1.5f));
            _maxSeats = _maxSeatsInWidth * _maxSeatsInHeight;

            _maxSeatSize = _defaultCardSize * _maxCardSizeScale;
            _maxSeatsInWidthByMaxSeatSize = (int)((_thisWidth - _maxSeatSize.x / 2f) / (_maxSeatSize.x * 1.5f));

            _lastIndex = -1;
            //_offset = 0f;
            //_maxCardsInRow = Convert.ToInt32((ROTransform.GetWidth() - Indent * 2 + _defaultCardSize.x) / (_defaultCardSize.x * 3f));
            //_maxCardsInColumn = Convert.ToInt32((ROTransform.GetHeight() + _defaultCardSize.y) / (_defaultCardSize.y * 3f));

            //Debug.Log("_maxCardsInRow " + _maxCardsInRow + ", _maxCardsInColumn " + _maxCardsInColumn + ", all: " + _maxCardsInRow * _maxCardsInColumn);
            _seats = new LookCardMenuSeat[_maxSeats];

            for (int i = 0; i < _maxSeats; i++)
            {
                LookCardMenuSeat seat = Instantiate(_seatTemplate, _seatContainer.GetTransform());
                _seats[i] = seat;
                seat.Init(cardDescription, bigCard);
            }

            _isComplete = true;
        }

        //private int FindCountSeats(Vector2 seatSize)
        //{
        //    int countInWidth = Convert.ToInt32((_thisWidth + seatSize.x / 2f) / (seatSize.x * 1.5f));
        //    int countInHeight = Convert.ToInt32((_thisHeight + seatSize.y / 2f) / (seatSize.y * 1.5f));

        //    return countInWidth * countInHeight;
        //}

        public void Activate(LookCardMenuSeatPanelActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;
            _isComplete = false;
            gameObject.SetActive(true);


            Card[] cards = data.Cards.ToArray();
            _lastIndex = -1;

            int countCards = cards.Length;

            if (countCards > _maxSeats)
                countCards = _maxSeats;

            _cards = new Card[countCards];

            for (int i = 0; i < countCards; i++)
            {
                _cards[i] = cards[i];
            }

            //for (int i = 0; i < _cards.Length; i++)
            //{
            //    _seats[i].SetCard(_cards[i]);
            //}

            //SortSeats();
            InitIndentsAndSizes();
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

            //foreach (LookCardMenuSeat seat in _seats)
            //{
            //    seat.Reset();
            //}

            _isComplete = true;
        }

        //private void SortSeats()
        //{
        //    float startPositionX;
        //    Vector3 seatPosition;

        //    int cardsCount = _cards.Count;
        //    // Кол-во карт в строке =FullWidth / (width карты + offset(думаю, полwidthкарты)) + учитывать offset-ы со сторон, тоже наверное полwidthкарты

        //    if (cardsCount > _maxCardsInRow * _maxCardsInColumn)
        //        cardsCount = _maxCardsInRow * _maxCardsInColumn;



        //    if (cardsCount % 2 == 1)
        //    {
        //        startPositionX = (cardsCount / 2 * _offset) * -1;
        //    }
        //    else
        //    {
        //        startPositionX = ((cardsCount / 2 - 1) * _offset + _offset / 2) * -1;
        //    }

        //    for (int i = 0; i < cardsCount; i++)
        //    {
        //        seatPosition = new Vector3(startPositionX + _offset * i, _positionY);

        //        _seats[i].SetLocalPositionValues(seatPosition, Quaternion.identity.eulerAngles);
        //    }
        //}

        private void InitIndentsAndSizes()
        {
            FindColumnsAndRowsCount();
            //FindIndents();

            //for (int i = 0; i < _cards.Count; i++)
            //{
            //    _seats[i].SetLocalPositionValues(CalcNumberPosition(i + 1, _cards.Count), Quaternion.identity.eulerAngles);
            //    //_selectNumbers[i].Init(CardNumbers[i], CalcNumberPosition(i + 1), new Vector2(NumberWidht, NumberHeight));
            //}

            //foreach (SelectNumber selectNumber in _selectNumbers)
            //{
            //    selectNumber.Init(number, CalcNumberPosition(number), new Vector2(_data.NumberWidht, _data.NumberHeight));
            //    number++;
            //}
        }

        //private Vector2 CalcNumberPosition(int cardNumber, int maxCards)
        //{
        //    if (cardNumber < 1 && cardNumber > maxCards)
        //    {
        //        throw new ArgumentOutOfRangeException($"Такой карты нет! cardNumber: {cardNumber}. maxCards: {maxCards}");
        //    }
        //    ////int row = (number - 1) / _columnsCount + 1;
        //    //int row = (cardNumber - 1) / ColumnsCount + 1;
        //    ////int column = ((number - 1) % _columnsCount) + 1;
        //    //int column = ((cardNumber - 1) % ColumnsCount) + 1;
        //    ////float x = Indent + _columnsIndent * column + NumberWidht / 2 + NumberWidht * (column - 1) - _maxWidth / 2;
        //    //float x = Indent + ColumnsCount * column + NumberWidht / 2 + NumberWidht * (column - 1) - _maxWidth / 2;
        //    ////float y = Indent * (-1) + _maxHeight - (_rowsIndent * row + NumberHeight / 2 + NumberHeight * (row - 1)) - _maxHeight / 2;
        //    //float y = Indent * (-1) + _maxHeight - (_rowsIndent * row + NumberHeight / 2 + NumberHeight * (row - 1)) - _maxHeight / 2;
        //    //Vector3 position = new Vector2(x, y);
        //    //Debug.Log("row: " + row);
        //    //Debug.Log("column: " + column);
        //    //Debug.Log("x: " + x);
        //    //Debug.Log("y: " + y);
        //    //Debug.Log("position: " + position);

        //    if ()

        //    return position;
        //}

        //private void FindIndents()
        //{
        //    //ScreenView.GetFactorX();
        //    //int maxHeight = Screen.height;
        //    //int maxWidth = Screen.width;
        //    _maxHeight = _rectTransform.rect.height;
        //    _maxWidth = _rectTransform.rect.width;
        //    float freeHeight = (_maxHeight - Indent * 2) - (NumberHeight * _rowsCount);
        //    float freeWidth = (_maxWidth - Indent * 2) - (NumberWidht * _columnsCount);
        //    _columnsIndent = freeWidth / (_columnsCount + 2 - 1);
        //    _rowsIndent = freeHeight / (_rowsCount + 2 - 1);

        //    Debug.Log("_maxHeight: " + _maxHeight);
        //    Debug.Log("_maxWidth: " + _maxWidth);
        //    Debug.Log("_columnsIndent: " + _columnsIndent);
        //    Debug.Log("_rowsIndent: " + _rowsIndent);
        //}

        private void SeatCards(int countInRow, int countInColumn, int currentRow, Vector2 cardSize, float cardIndentHeight,
            float cardIndentWidth, float edgeIndentHeight, float edgeIndentWidth)
        {
            //Debug.Log("countInRow: " + countInRow);
            //Debug.Log("countInColumn: " + countInColumn);
            //Debug.Log("cardSize: " + cardSize);
            //Debug.Log("cardIndentHeight: " + cardIndentHeight);
            //Debug.Log("cardIndentWidth: " + cardIndentWidth);
            //Debug.Log("edgeIndentHeight: " + edgeIndentHeight);
            //Debug.Log("edgeIndentWidth: " + edgeIndentWidth);


            //if (countInRow * countInColumn != _cards.Length)
            //    throw new Exception($"Ошибка логики расставления карт в {nameof(LookCardMenuSeatPanel)}");

            //for (int row = 0; row < countInColumn; row++)
            //{
                for (int column = 0; column < countInRow; column++)
                {
                    _lastIndex++;
                    int currentIndex = _lastIndex;

                    Card card = _cards[currentIndex];

                    float positionX = edgeIndentWidth + column * cardIndentWidth + column * cardSize.x
                        + cardSize.x / 2f; // Тк pivot в центре
                    float localPositionX = positionX - _thisWidth / 2f; // Превращаем позицию расчета в local

                    float positionY = edgeIndentHeight + currentRow * cardIndentHeight + currentRow * cardSize.y
                        + cardSize.y / 2f;
                    float localPositionY = positionY - _thisHeight / 2f;

                    Vector2 position = new Vector2(localPositionX, localPositionY);

                    //Debug.Log("position: " + position);

                    _seats[currentIndex].SetLocalPositionValues(position, Quaternion.identity.eulerAngles);
                    _seats[currentIndex].SetCard(card, cardSize);
                }
            //}
        }

        private void FindColumnsAndRowsCount()
        {
            //int countAll = _selectNumbers.Length;
            int countAll = _cards.Length;

            int qnty;
            int countInRow;
            int countInColumn = (countAll - 1) / _maxSeatsInWidth + 1;
            float scale;
            Vector2 cardSize;
            float cardIndentHeight;
            float cardIndentWidth;
            float edgeIndentHeight; 
            float edgeIndentWidth;

            if (countAll <= _maxSeatsInWidthByMaxSeatSize) // Если все карты помещаются в 1 строку большим размером
            {
                countInRow = countAll;
                //countInColumn = 1;

                scale = _maxCardSizeScale;
                cardSize = _defaultCardSize * scale;

                cardIndentHeight = cardSize.y / 2f;
                cardIndentWidth = cardSize.x / 2f;
                edgeIndentHeight = (_thisHeight
                    - cardSize.y * countInColumn
                    - cardIndentHeight * (countInColumn - 1)
                    ) / 2;

                edgeIndentWidth = (_thisWidth
                    - cardSize.x * countInRow
                    - cardIndentWidth * (countInRow - 1)
                    ) / 2;

                SeatCards(countInRow, countInColumn, 0, cardSize, cardIndentHeight, cardIndentWidth, edgeIndentHeight, edgeIndentWidth);
                return;
            }

            if (countInColumn == 1) // Если все карты помещаются в 1 строку любым доступным размером
            {
                countInRow = countAll;
                //countInColumn = 1;

                //float maxSeatsInWidth = (_thisWidth - _minSeatSize.x / 2f)
                //    /
                //    (_minSeatSize.x * 1.5f);

                //width *  ( 3 * count + 1) = 2 * tw

                // Формулы, обратные _maxSeatsInHeight = (int)((_thisHeight - _minSeatSize.y / 2f) / (_minSeatSize.y * 1.5f));
                float width = 2f * _thisWidth / (3f * countInRow + 1f);
                float height = width * _defaultCardSize.y / _defaultCardSize.x;

                cardSize = new Vector2(width, height);

                Debug.Log("cardSize: " + cardSize);

                //scale = _minCardSizeScale;
                //cardSize = _defaultCardSize * scale;

                cardIndentHeight = cardSize.y / 2f;
                cardIndentWidth = cardSize.x / 2f;
                edgeIndentHeight = (_thisHeight
                    - cardSize.y * countInColumn
                    - cardIndentHeight * (countInColumn - 1)
                    ) / 2;

                edgeIndentWidth = (_thisWidth
                    - cardSize.x * countInRow
                    - cardIndentWidth * (countInRow - 1)
                    ) / 2;

                SeatCards(countInRow, countInColumn, 0, cardSize, cardIndentHeight, cardIndentWidth, edgeIndentHeight, edgeIndentWidth);
                return;
            }

            if (countInColumn > 1)
            {
                countInRow = countAll / countInColumn;
                int restInRow = countAll % countInColumn;
                int rowWhereIncreaseCountCards = countInColumn - restInRow;

                float widthByWidth = 2f * _thisWidth / (3f * countInRow + 1f);
                float heightByWidth = widthByWidth * _defaultCardSize.y / _defaultCardSize.x;

                float heightByHeight = 2f * _thisHeight / (3f * countInColumn + 1f);
                float widthByHeight = heightByHeight * _defaultCardSize.x / _defaultCardSize.y;

                float height = heightByHeight > heightByWidth ? heightByWidth : heightByHeight;
                float width = widthByHeight > widthByWidth ? widthByWidth : widthByHeight;

                cardSize = new Vector2(width, height);

                for (int row = 0; row < countInColumn; row++)
                {
                    if (row == rowWhereIncreaseCountCards)
                        countInRow++;

                    //Debug.Log("cardSize: " + cardSize);

                    //scale = _minCardSizeScale;
                    //cardSize = _defaultCardSize * scale;

                    cardIndentHeight = cardSize.y / 2f;
                    cardIndentWidth = cardSize.x / 2f;
                    edgeIndentHeight = (_thisHeight
                        - cardSize.y * countInColumn
                        - cardIndentHeight * (countInColumn - 1)
                        ) / 2;

                    edgeIndentWidth = (_thisWidth
                        - cardSize.x * countInRow
                        - cardIndentWidth * (countInRow - 1)
                        ) / 2;

                    SeatCards(countInRow, countInColumn, row, cardSize, cardIndentHeight, cardIndentWidth, edgeIndentHeight, edgeIndentWidth);
                }

                return;
            }



            //int qnty = Convert.ToInt32(Math.Sqrt(countAll));
            //int firstSize;
            //int secondSize;
            //for (int i = qnty; i > 0; i--)
            //{
            //    //if (countAll % i == 0 && countAll / i <= 10)
            //    if (countAll % i == 0 && countAll / i <= 5)
            //    {
            //        firstSize = countAll / i;
            //        secondSize = i;
            //        if (firstSize > secondSize)
            //        {
            //            _rowsCount = secondSize;
            //            _columnsCount = firstSize;
            //        }
            //        else
            //        {
            //            _rowsCount = firstSize;
            //            _columnsCount = secondSize;
            //        }
            //        _lastRowColumnsCount = _columnsCount;
            //        CheckRightCalcColumnsAndRows();
            //        return;
            //    }
            //}
            //_columnsCount = qnty;
            //_rowsCount = qnty;
            //while (countAll - _columnsCount * (_rowsCount - 1) > _columnsCount)
            //{
            //    _columnsCount++;
            //}
            //_lastRowColumnsCount = countAll - _columnsCount * (_rowsCount - 1);
            //CheckRightCalcColumnsAndRows();
        }

        //private void CheckRightCalcColumnsAndRows()
        //{
        //    //if ((_rowsCount - 1) * _columnsCount + _lastRowColumnsCount != _selectNumbers.Length)
        //    if ((_rowsCount - 1) * _columnsCount + _lastRowColumnsCount != _cards.Count)
        //    {
        //        //throw new Exception($"Ошибка расчетов. Всего мест: {_selectNumbers.Length}. Columns = {_columnsCount}. Rows = {_rowsCount}. LastRowColumns = {_lastRowColumnsCount}");
        //        throw new Exception($"Ошибка расчетов. Всего мест: {_cards.Count}. Columns = {_columnsCount}. Rows = {_rowsCount}. LastRowColumns = {_lastRowColumnsCount}");
        //    }
        //}

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
