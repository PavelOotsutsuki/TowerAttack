using GameFields.Persons.SelectMenues.Commons;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceNumberPanelPlayer : SelectNumberPanelPlayer
    {
        protected override SetSelectResultData CreateSetSelectResultData(ResultType resultType)
        {
            string defaultText = "Выбрано: ";
            string message = defaultText;

            foreach (SelectNumber selectNumber in CurrentSelectedNumbers)
            {
                if (message != defaultText)
                    message += ", ";

                message += selectNumber.Number.ToString();
            }

            SetChoiceResultData data = new SetChoiceResultData(resultType, message);

            return data;
        }
    }
}