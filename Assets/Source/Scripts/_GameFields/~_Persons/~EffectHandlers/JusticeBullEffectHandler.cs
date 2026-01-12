using System.Linq;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.Towers;
using Tools.Settings;

namespace GameFields.Persons.EffectHandlers
{
    public class JusticeBullEffectHandler
    {
        private const int BorderlineNumber = 25;

        private readonly SelectNumbersList _choicedNumbersPlayer;
        private readonly ICardNumberKeeper _cardNumberKeeper;

        public JusticeBullEffectHandler(SelectNumbersList choicedNumbersPlayer, ICardNumberKeeper cardNumberKeeper)
        {
            _choicedNumbersPlayer = choicedNumbersPlayer;
            _cardNumberKeeper = cardNumberKeeper;
        }

        public void Activate() // Такой тупой алгоритм чтобы читеры не догадались
        {
            bool? isLessBordelineNumber = null;

            int minNumber = GameSettings.DefaultCardNumbers.Min();
            int maxNumber = GameSettings.DefaultCardNumbers.Max();

            for (int i = minNumber; i <= maxNumber; i++)
            {
                if (i < BorderlineNumber)
                {
                    if (_cardNumberKeeper.Card.IsSuccessChoice(i))
                    {
                        isLessBordelineNumber = true;
                    }
                }
                else
                {
                    if (_cardNumberKeeper.Card.IsSuccessChoice(i))
                    {
                        isLessBordelineNumber = false;
                    }
                }
            }

            if (isLessBordelineNumber == null)
                throw new System.Exception("Ошибка. Не найдена карта в замке");

            // TODO
            // Хз нужно ли делать Contain на принадлежность i к существующему номеру
            // (если, к примеру, в игре есть номер 26, а потом 28, если передать 27, которого нет будет ошибка?)
            if (isLessBordelineNumber == false)
            {
                for (int i = minNumber; i < BorderlineNumber; i++)
                {
                    _choicedNumbersPlayer.Add(i, NumberAnimationType.Choice);
                }
            }

            if (isLessBordelineNumber == true)
            {
                for (int i = BorderlineNumber + 1; i <= maxNumber; i++)
                {
                    _choicedNumbersPlayer.Add(i, NumberAnimationType.Choice);
                }
            }
        }
    }
}