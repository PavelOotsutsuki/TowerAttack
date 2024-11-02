using Tools;
using Tools.UI.Fadings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.StartFights
{
    [RequireComponent(typeof(FadableLabel))]
    public class WaitEnemySolutionLabel : MonoBehaviour, ICompletable, IViewable
    {
        [SerializeField] private FadableLabel _fadableLabel;

        private bool _isWasStarted;

        public bool IsComplete => _fadableLabel.IsComplete;

        public void Init()
        {
            _isWasStarted = false;

            _fadableLabel.Init();
        }

        public void Show()
        {
            _isWasStarted = true;

            _fadableLabel.Show();
        }

        public void Hide()
        {
            if (_isWasStarted == false)
                return;

            _fadableLabel.Hide();

            _isWasStarted = false;
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFadableLabel();
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private void DefineFadableLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }

        #endregion
    }
}