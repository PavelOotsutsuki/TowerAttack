using Tools.UI;

namespace GameFields.Persons.SelectMenues.Attacks
{
    //[RequireComponent(typeof(FadablePanel))]
    public class AttackMenuPanel : FadablePanel //MonoBehaviour, ICompletable, IWorkable, IAutomaticFillComponents
    {
        //[SerializeField] private FadablePanel _fadablePanel;

        //public bool IsComplete => _fadablePanel.IsComplete;

        //public void Init()
        //{
        //    _fadablePanel.Init();
        //}

        //public void Activate()
        //{
        //    _fadablePanel.Show();
        //}

        //public void Deactivate()
        //{
        //    _fadablePanel.Hide();
        //}

        //#region AutomaticFillComponents

        //[ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenuPanel))]
        //public void DefineAllComponents()
        //{
        //    DefineFadablePanel();
        //}

        //[ContextMenu(nameof(DefineFadablePanel))]
        //private void DefineFadablePanel()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        //}

        //#endregion
        //private const float MaxAlpha = 255f;
        //private const float DeactiveAlpha = 0f;

        //[SerializeField] private Image _panel;
        //[SerializeField] private float _activateDuration = 1f;
        //[SerializeField] private float _deactivateDuration = 2f;
        //[SerializeField, Range(DeactiveAlpha, MaxAlpha)] private float _activeAlpha = 248f;

        //private Action _deactivateCallback;

        //public void Init(Action deactivateCallback)
        //{
        //    _deactivateCallback = deactivateCallback;

        //    _panel.raycastTarget = false;

        //    Color startColor = new Color(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha);

        //    _panel.color = startColor;
        //}

        //public void Activate()
        //{
        //    _panel.raycastTarget = true;

        //    Color activateColor = new Color(_panel.color.r, _panel.color.g, _panel.color.b, _activeAlpha / MaxAlpha);

        //    _panel.DOColor(activateColor, _activateDuration);
        //}

        //public void Deactivate()
        //{
        //    Color deactivateColor = new Color(_panel.color.r, _panel.color.g, _panel.color.b, DeactiveAlpha / MaxAlpha);

        //    _panel.DOColor(deactivateColor, _deactivateDuration)
        //    .OnComplete(() =>
        //    {
        //        _panel.raycastTarget = false;
        //        _deactivateCallback.Invoke();
        //    });
        //}

        //#region AutomaticFillComponents

        //[ContextMenu(nameof(DefineAllComponents))]
        //private void DefineAllComponents()
        //{
        //    DefinePanel();
        //}

        //[ContextMenu(nameof(DefinePanel))]
        //private void DefinePanel()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _panel, ComponentLocationTypes.InThis);
        //}

        //#endregion
    }
}