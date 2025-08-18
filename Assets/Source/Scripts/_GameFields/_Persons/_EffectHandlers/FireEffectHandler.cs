using GameFields.Persons.DrawCards;

namespace GameFields.Persons.EffectHandlers.Fires
{
    public class FireEffectHandler : IFireEffectHandler
    {
        private readonly IFireDrawCardAnimationSetter _cardAnimationManager;

        private int _countTurns;

        public FireEffectHandler(IFireDrawCardAnimationSetter cardAnimationManager)
        {
            _cardAnimationManager = cardAnimationManager;

            _countTurns = 0;
        }

        public bool IsFireMode => _countTurns > 0;

        public void Activate(int countTurns)
        {
            _countTurns = countTurns
                + 1; // +1 чтобы нейтрализовать эффект "в начале хода"

            _cardAnimationManager.SetFireMode();
        }

        public void OnStartTurn()
        {
            if (_countTurns > 0)
            {
                _countTurns--;

                if (_countTurns == 0)
                {
                    _cardAnimationManager.SetSimpleMode();
                }
            }
        }
    }
}