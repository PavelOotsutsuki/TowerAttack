using System;
using Tools.Utils.Orthographyes;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class DefaultSelectMenuLabelTextLogic : SelectMenuLabelTextLogic
    {
        private readonly Func<int> _needForActivateGetter;

        public DefaultSelectMenuLabelTextLogic(Func<int> needForActivateGetter) : base()
        {
            _needForActivateGetter = needForActivateGetter;
        }

        public override string CreateLabelText()
        {
            int needForActivate = _needForActivateGetter.Invoke();

            string labelText = "Выберите " + needForActivate.ToString() + " ";

            labelText += Orthography.GetWordByNumber(WordType.Numbers, needForActivate);

            return labelText;
        }
    }
}