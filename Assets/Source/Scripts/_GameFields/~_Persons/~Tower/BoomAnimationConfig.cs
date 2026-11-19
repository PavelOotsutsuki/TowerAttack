using Tools;

namespace GameFields.Persons.Towers
{
    public class BoomAnimationConfig: IData
    {
        private readonly TowerSeat _towerSeat;
        private readonly Stone[] _stones;
        private readonly ReadOnlyRectTransform _rectTransform;
        private readonly BoomAnimationData _data;

        public BoomAnimationConfig(TowerSeat towerSeat, Stone[] stones, ReadOnlyRectTransform rectTransform,
            BoomAnimationData data)
        {
            _towerSeat = towerSeat;
            _stones = stones;
            _rectTransform = rectTransform;
            _data = data;
        }

        public TowerSeat TowerSeat => _towerSeat;
        public Stone[] Stones => _stones;
        public ReadOnlyRectTransform RectTransform => _rectTransform;
        public BoomAnimationData Data => _data;
    }
}