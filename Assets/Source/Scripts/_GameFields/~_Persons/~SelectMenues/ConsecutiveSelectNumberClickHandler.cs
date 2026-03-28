using System.Collections.Generic;
using Tools;

namespace GameFields.Persons.SelectMenues
{
    public class ConsecutiveSelectNumberClickHandler : SelectNumberClickHandler
    {
        private readonly IReadOnlyList<SelectNumber> _selectNumbers;

        public ConsecutiveSelectNumberClickHandler(int needForActivate, IWorkable selectButton, IReadOnlyList<SelectNumber> selectNumbers):
            base(needForActivate, selectButton)
        {
            _selectNumbers = selectNumbers;
        }

        public override bool CanBeClicked(SelectNumber currentNumber)
        {
            // Проверки начало
            if (currentNumber.Number + NeedForActivate - 1 > _selectNumbers.Count)
                return false;

            for (int i = 1; i < NeedForActivate; i++)
            {
                if (_selectNumbers[currentNumber.Number + i - 1].IsDisable)
                    return false;
            }
            // Проверки конец

            ClearAllNumbers();

            for (int i = 1; i < NeedForActivate; i++)
            {
                _selectNumbers[currentNumber.Number + i - 1].OnPointerClick(null);
            }

            return true;
        }

        public override void OnEnterClick()
        {
            ActivateCounter = NeedForActivate;
        }

        public override void OnExitClick()
        {
            Reset();
        }

        private void ClearAllNumbers()
        {
            foreach (SelectNumber selectNumber in _selectNumbers)
            {
                if (selectNumber.IsClicked)
                {
                    selectNumber.OnPointerClick(null);
                }
            }
        }
    }
}