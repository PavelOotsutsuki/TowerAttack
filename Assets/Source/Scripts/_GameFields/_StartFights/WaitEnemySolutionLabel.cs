using Tools;
using Tools.UI.Fadings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.StartFights
{
    [RequireComponent(typeof(FadableLabel))]
    public class WaitEnemySolutionLabel : MonoBehaviour, ICompletable
    {
        [SerializeField] private FadableLabel _fadableLabel;

        public bool IsWasStarted { get; private set; }

        public bool IsComplete => _fadableLabel.IsComplete;

        public void Init()
        {
            IsWasStarted = false;

            _fadableLabel.Init();
        }

        public void Show()
        {
            IsWasStarted = true;

            _fadableLabel.Show();
        }

        public void Hide()
        {
            if (IsWasStarted == false)
                return;

            _fadableLabel.Hide();

            IsWasStarted = false;
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
