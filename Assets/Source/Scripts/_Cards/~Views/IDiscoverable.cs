using Tools;

namespace Cards.Views
{
    public interface IDiscoverable : IReadOnlyRectTransformable
    {
        public CardViewData ViewData { get; }
    }
}