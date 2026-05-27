using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.StartFights
{
    //[RequireComponent(typeof(FadableLabel))]
    public class WaitEnemySolutionLabel : FadableLabel//MonoBehaviour, ICompletable, IViewable, IAutomaticFillComponents
    {
        //[SerializeField] private FadableLabel _fadableLabel;

        //private bool _isWasStarted;

        //public bool IsComplete => _fadableLabel.IsComplete || _isWasStarted == false;

        //public bool? IsShown => throw new System.NotImplementedException();

        //public void Init()
        //{
        //    _isWasStarted = false;

        //    _fadableLabel.Init();
        //}

        //public void Show()
        //{
        //    _isWasStarted = true;

        //    _fadableLabel.Show();
        //}

        //public void Hide()
        //{
        //    if (_isWasStarted == false)
        //        return;

        //    _fadableLabel.Hide();

        //    _isWasStarted = false;
        //}

        //#region AutomaticFillComponents

        //[ContextMenu(nameof(DefineAllComponents) + nameof(WaitEnemySolutionLabel))]
        //public void DefineAllComponents()
        //{
        //    DefineFadableLabel();
        //}

        //[ContextMenu(nameof(DefineFadableLabel))]
        //private void DefineFadableLabel()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        //}

        //#endregion
    }
}