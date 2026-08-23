using System.Linq;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.Towers;
using Tools.Settings;

namespace GameFields.Persons.EffectHandlers
{
    public class FallenGuardianEffectHandler
    {
        private readonly SelectNumbersList _choicedNumbersPerson;
        private readonly ICardNumberKeeper _cardNumberKeeper;

        public FallenGuardianEffectHandler(SelectNumbersList choicedNumbersPerson, ICardNumberKeeper cardNumberKeeper)
        {
            _choicedNumbersPerson = choicedNumbersPerson;
            _cardNumberKeeper = cardNumberKeeper;
        }

        public void Activate() // Такой тупой алгоритм чтобы читеры не догадались
        {
            bool? isOdd = null;

            int minNumber = GameSettings.DefaultCardNumbers.Min();
            int maxNumber = GameSettings.DefaultCardNumbers.Max();

            for (int i = minNumber; i <= maxNumber; i++)
            {
                if (i % 2 == 1)
                {
                    if (_cardNumberKeeper.Card.IsSuccessChoice(i))
                    {
                        isOdd = true;
                    }
                }
                else
                {
                    if (_cardNumberKeeper.Card.IsSuccessChoice(i))
                    {
                        isOdd = false;
                    }
                }
            }

            if (isOdd == null)
                throw new System.Exception("Ошибка. Не найдена карта в замке");

            // TODO
            // Хз нужно ли делать Contain на принадлежность i к существующему номеру
            // (если, к примеру, в игре есть номер 26, а потом 28, если передать 27, которого нет будет ошибка?)
            if (isOdd == false)
            {
                for (int i = minNumber; i <= maxNumber; i++)
                {
                    if (i % 2 == 1)
                        //_choicedNumbersPerson.Add(i, NumberAnimationType.Choice);
                        _choicedNumbersPerson.Add(i);
                }
            }

            if (isOdd == true)
            {
                for (int i = minNumber; i <= maxNumber; i++)
                {
                    if (i % 2 == 0)
                        //_choicedNumbersPerson.Add(i, NumberAnimationType.Choice);
                        _choicedNumbersPerson.Add(i);
                }
            }
        }
    }
}