using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields
{
    public class EndTurnButton : MonoBehaviour, IPointerClickHandler
    {
        private IDrawCardManager _drawHandler;

        public void Init(IDrawCardManager drawHandler)
        {
            _drawHandler = drawHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _drawHandler.DrawCard();
        }
    }
}
