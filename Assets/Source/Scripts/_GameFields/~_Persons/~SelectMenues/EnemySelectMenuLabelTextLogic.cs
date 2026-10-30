namespace GameFields.Persons.SelectMenues
{
    public class EnemySelectMenuLabelTextLogic : SelectMenuLabelTextLogic
    {
        private readonly string _defaultText;

        public EnemySelectMenuLabelTextLogic(string defaultText) : base()
        {
            _defaultText = defaultText;
        }

        public override string CreateLabelText()
        {
            return _defaultText;
        }
    }
}