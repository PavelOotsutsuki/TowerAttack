using System.Collections;
using System.Collections.Generic;
using GameFields.DiscardPiles;
using GameFields.EndTurnButtons;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace GameFields.FirstTurns
{
    public class FirstTurn : MonoBehaviour
    {
        [SerializeField] private FirstTurnPanel _firstTurnPanel;
        [SerializeField] private FirstTurnLabel _firstTurnLabel;

        public void Init()
        {
            _firstTurnPanel.Init();
            _firstTurnLabel.Init();
        }

        public IEnumerator Activate()
        {
            gameObject.SetActive(true);

            yield return _firstTurnPanel.Activate();
            _firstTurnLabel.Activate().ToUniTask();
        }

        public IEnumerator Deactivate()
        {
            yield return _firstTurnPanel.Deactivate();

            gameObject.SetActive(false);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFirstTurnPanel();
            DefineFirstTurnLabel();
        }

        [ContextMenu(nameof(DefineFirstTurnPanel))]
        private void DefineFirstTurnPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurnPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFirstTurnLabel))]
        private void DefineFirstTurnLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurnLabel, ComponentLocationTypes.InChildren);
        }
    }
}
