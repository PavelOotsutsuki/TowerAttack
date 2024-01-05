using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields
{
    public class EndTurnButton : MonoBehaviour, IPointerClickHandler
    {
        private IEndTurnHandler _drawHandler;

        public void Init(IEndTurnHandler drawHandler)
        {
            _drawHandler = drawHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _drawHandler.OnEndTurn();
        }
    }
}
