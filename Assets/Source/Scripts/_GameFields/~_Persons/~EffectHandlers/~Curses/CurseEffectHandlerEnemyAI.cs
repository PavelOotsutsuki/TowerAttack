using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Zenject;

namespace GameFields.Persons.EffectHandlers.Curses
{
    public class CurseEffectHandlerEnemyAI : CurseEffectHandler
    {
        private const string PersonDefaultText = "Наложенное Вами проклятье:";

        private readonly InformationLabel _informationLabel;
        private readonly SignalBus _bus;

        public CurseEffectHandlerEnemyAI(ICardNumberKeeper enemyTower, InformationLabel informationLabel,
            ConfirmableNumbers confirmableNumbersPlayer, SelectNumbersList cursedNumbersPlayer, SignalBus bus) :
            base(enemyTower, informationLabel, confirmableNumbersPlayer, cursedNumbersPlayer)
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
            _bus.Fire(new PushStepSignalEnemyAI(_informationLabel));
        }
    }
}