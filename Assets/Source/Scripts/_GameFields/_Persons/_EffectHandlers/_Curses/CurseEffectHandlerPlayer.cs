using GameFields.InformationLabels;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Zenject;

namespace GameFields.Persons.EffectHandlers.Curses
{
    public class CurseEffectHandlerPlayer : CurseEffectHandler
    {
        private const string PersonDefaultText = "Наложенное противником проклятье:";

        private readonly InformationLabel _informationLabel;
        private readonly SignalBus _bus;

        public CurseEffectHandlerPlayer(ICardNumberKeeper playerTower, InformationLabel informationLabel,
            ConfirmableNumbers confirmableNumbersEnemyAI, SelectNumbersList cursedNumbersEnemyAI, SignalBus bus) :
            base(playerTower, informationLabel, confirmableNumbersEnemyAI, cursedNumbersEnemyAI)
        {
            _informationLabel = informationLabel;
            _bus = bus;
        }

        protected override string GetPersonFeature()
        {
            return PersonDefaultText;
        }

        protected override void PushStep()
        {
            _bus.Fire(new PushStepSignalPlayer(_informationLabel));
        }
    }
}