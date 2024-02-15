using System.Collections;
using System.Collections.Generic;
using GameFields.DiscardPiles;
using GameFields.EndTurnButtons;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.FirstTurns
{
    public class FirstTurn : MonoBehaviour
    {
        private const float DeactiveAlpha = 0f;
        private const float ActiveAlpha = 211f;

        [SerializeField] private FirstTurnPanel _firstTurnPanel;

        public void Init()
        {
            _firstTurnPanel.Init();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFirstTurnPanel();
        }

        [ContextMenu(nameof(DefineFirstTurnPanel))]
        private void DefineFirstTurnPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurnPanel, ComponentLocationTypes.InChildren);
        }
    }
}
