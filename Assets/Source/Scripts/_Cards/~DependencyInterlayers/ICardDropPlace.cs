using Tools;

namespace Cards.DependencyInterlayers
{
    public interface ICardDropPlace
    {
        public bool HasFreeSeat { get; }
        public ReadOnlyRectTransform RORTransform { get; }
    }
}