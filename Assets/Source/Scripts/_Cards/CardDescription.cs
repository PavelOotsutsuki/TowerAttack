using Tools.UI;

namespace Cards
{
    public class CardDescription : FadableLabel
    {
        public override void Init()
        {
            gameObject.SetActive(true);

            base.Init();
        }
        //[SerializeField] private FadableLabel _fadableLabel;

        //public void Init()
        //{
        //    _fadableLabel.Init();
        //}

        //public void Show(FadableLabelActivateData data)
        //{
        //    _fadableLabel.Show(data);
        //}

        //public void Hide()
        //{
        //    _fadableLabel.Hide();
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponentsFadableLabel))]
        //private void DefineAllComponentsFadableLabel()
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