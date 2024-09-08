using Cards;
using GameFields;
using GameFields.DiscardPiles;
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
        private void Construct(SignalBus bus, Deck deck, SeatPool seatPool)
        {
            _screenRoot.Init();

            seatPool.Init();
            _endTurnButton.Init();
            _personCreator.Init(bus, deck, _endTurnButton, seatPool);

            _personsState = new PersonsState(_personCreator.CreatePlayer(), _personCreator.CreateEnemyAI());
            EffectFactory effectFactory = new EffectFactory(_personsState);

            _cardRoot.Init(effectFactory);
            deck.Init(_cardRoot.Cards);

            _gameFieldRoot.Init(_personsState);
        }

        #region AutomaticFillComponents

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

        #endregion
    }
}