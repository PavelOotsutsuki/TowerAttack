using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class DefaultSelectNumberClickHandler : SelectNumberClickHandler
    {
        public DefaultSelectNumberClickHandler(int needForActivate, IWorkable selectButton): base(needForActivate, selectButton)
        { }

        public override bool CanBeClicked(SelectNumber currentNumber)
        {
            return true;
        }

        public override void OnEnterClick()
        {
            ActivateCounter += 1;
        }

        public override void OnExitClick()
        {
            ActivateCounter -= 1;
        }
    }
}