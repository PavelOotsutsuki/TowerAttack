using Tools;

namespace Cards
{
    public interface IDiscoverable : IReadOnlyRectTransformable
    {
        public CardViewData ViewData { get; }
    }
}