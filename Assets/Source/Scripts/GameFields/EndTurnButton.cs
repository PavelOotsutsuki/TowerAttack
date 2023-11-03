using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields
{
    public class EndTurnButton : MonoBehaviour, IPointerClickHandler
    {
        private IDrawCardHandler _drawHandler;

        public void Init(IDrawCardHandler drawHandler)
        {
            _drawHandler = drawHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _drawHandler.DrawCard();
        }
    }
}
