using Cards;
using GameFields;
using GameFields.Discarding;
using GameFields.Effects;
using GameFields.EndTurnButtons;
using GameFields.Persons;
using GameFields.Seats;
using Tools;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private CardRoot _cardRoot;
        [SerializeField] private GameFieldRoot _gameFieldRoot;
        [SerializeField] private ScreenRoot _screenRoot;
        [SerializeField] private PersonCreator _personCreator;

        private PersonsState _personsState;
        
        [Inject]
        private void Construct(SignalBus bus, Deck deck, DiscardPile discardPile, SeatPool seatPool)
        {
            _personCreator.Init(bus, deck, _endTurnButton, seatPool);
            _endTurnButton.Init();
            _screenRoot.Init();

            seatPool.Init();
            discardPile.Init(seatPool);
            deck.Init(_cardRoot.Cards);

            _personsState = new PersonsState(_personCreator.CreatePlayer(), _personCreator.CreateEnemyAI());
            EffectFactory effectFactory = new(bus, deck, _personsState);
            _cardRoot.Init(effectFactory);

            _gameFieldRoot.Init(_personsState);
        }

        private void OnDestroy()
        {
            _personsState.Dispose();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardRoot();
            DefineGameFieldRoot();
            DefineScreenRoot();
        }

        [ContextMenu(nameof(DefineCardRoot))]
        private void DefineCardRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineGameFieldRoot))]
        private void DefineGameFieldRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _gameFieldRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineScreenRoot))]
        private void DefineScreenRoot()
        {
            AutomaticFillComponents.DefineComponent(this, ref _screenRoot, ComponentLocationTypes.InThis);
        }
    }
}