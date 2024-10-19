using Cards;
using GameFields;
using GameFields.Effects;
using GameFields.EndTurnButtons;
using GameFields.Persons;
using GameFields.Seats;
using Tools.Utils.FillComponents;
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
        private void Construct(SignalBus bus, Deck deck, SeatPool seatPool, CardDescription cardDescription)
        {
            _screenRoot.Init();

            seatPool.Init();
            _endTurnButton.Init();
            _personCreator.Init(bus, deck, _endTurnButton, seatPool);

            Player player = _personCreator.CreatePlayer();
            EnemyAI enemyAI = _personCreator.CreateEnemyAI();

            _personsState = new PersonsState(player, enemyAI);
            EffectFactory effectFactory = new EffectFactory(_personsState);

            _cardRoot.Init(effectFactory, cardDescription);
            deck.Init(_cardRoot.Cards);

            _gameFieldRoot.Init(_personsState, player, enemyAI);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineEndTurnButton();
            DefineCardRoot();
            DefineGameFieldRoot();
            DefineScreenRoot();
            DefinePersonCreator();
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private void DefineEndTurnButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
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

        [ContextMenu(nameof(DefinePersonCreator))]
        private void DefinePersonCreator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _personCreator, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}