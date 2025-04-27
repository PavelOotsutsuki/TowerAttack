using GameFields.Persons.SelectMenues.Commons;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceNumberPanelPlayer : SelectNumberPanelPlayer
    {
        protected override void ActivateNumber(SelectNumber target)
        {
            NumberAnimationType? numberAnimationType = null;

            if (SelectedNumbers.Contains(target))
            {
                numberAnimationType = SelectedNumbers.GetType(target.Number);
            }

            SelectNumberActivateData data = new SelectNumberActivateData(numberAnimationType);

            target.Activate(data);
        }

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

        protected override NumberAnimationType ConvertResultTypeToNumberAnimationType(ResultType resultType)
        {
            return NumberAnimationType.Choice;
        }

        //protected override void SetChoiceNumber(SelectNumber target, ResultType resultType)
        //{
        //    target.SetChoice(NumberAnimationType.Choice);
        //}
    }
}