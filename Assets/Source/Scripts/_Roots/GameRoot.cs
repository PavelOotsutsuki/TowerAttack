using System.Collections.Generic;
using Cards;
using GameFields;
using GameFields.Decks;
using GameFields.Effects;
using GameFields.EndTurnButtons;
using GameFields.LightControls;
using GameFields.Persons.Common;
using GameFields.Persons.Hands;
using GameFields.Seats;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using CanvasSortOrders;
using GameFields.InformationLabels;
using GameFields.DiscardPiles;

namespace Roots
{
    public class GameRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private CardRoot _cardRoot;
        [SerializeField] private GameFieldRoot _gameFieldRoot;
        [SerializeField] private ScreenRoot _screenRoot;
        [SerializeField] private FontRoot _fontRoot;
        [SerializeField] private PersonCreator _personCreator;
        [SerializeField] private ObjectsLightControlsCreator _lightControlsCreator;
        [SerializeField] private SpeedUpButtonSortOrder _speedUpButtonSortOrder;
        [SerializeField] private ForgingZone _forgingZone;

        private PersonsState _personsState;

        [Inject]
        private void Construct(SignalBus bus, Deck deck, SeatPool seatPool, CardDescription cardDescription, HandPlayer handPlayer,
            InformationLabel informationLabel, DiscardPile discardPile)
        {
            GameFieldGC.GCOFF();

            _screenRoot.Init();
            informationLabel.Init();
            _fontRoot.Init();

            seatPool.Init();
            _endTurnButton.Init();

            _lightControlsCreator.Init();
            _speedUpButtonSortOrder.Init();

            CardDragAndDropLightController cardDragAndDropLightController = _lightControlsCreator.CreateCardDragAndDropLightController();

            //Destroy(_lightControlsCreator.gameObject);

            CardDragAndDropHandler cardDragAndDropHandler = new CardDragAndDropHandler(handPlayer, handPlayer,
                cardDragAndDropLightController, _speedUpButtonSortOrder);

            _personCreator.Init(bus, deck, _endTurnButton, seatPool, cardDragAndDropHandler, cardDragAndDropLightController,
                informationLabel, _forgingZone);

            Player player = _personCreator.CreatePlayer();
            EnemyAI enemyAI = _personCreator.CreateEnemyAI();
            CardLocationViewRoot viewRoot = _personCreator.CreateCardLocationViewRoot(_cardRoot);

            Destroy(_personCreator.gameObject);

            _personsState = new PersonsState(player, enemyAI);
            _forgingZone.Init(discardPile, _personsState);
            EffectFactory effectFactory = new EffectFactory(_personsState, viewRoot, informationLabel);

            _cardRoot.Init(effectFactory, cardDescription, cardDragAndDropHandler);
            deck.Init(_cardRoot.Cards);

            _gameFieldRoot.Init(_personsState, player, enemyAI, bus, seatPool);
        }

        public void OnDestroy()
        {
            GameFieldGC.GCON();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineGameComponents))]
        private void DefineGameComponents()
        {
            List<ComponentAttachInfo> infos = new List<ComponentAttachInfo>();

            List<ComponentAttachInfo> exists = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> success = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> error = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> successButSoMuch = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> sceneNotExists = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> successForArray = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> noWay = new List<ComponentAttachInfo>();

            IAutomaticFillComponents[] gameComponents = GetComponentsInChildren<IAutomaticFillComponents>(true);
            int allComponents = 0;

            foreach (IAutomaticFillComponents component in gameComponents)
            {
                infos.AddRange(component.DefineAllComponents());
                allComponents++;
            }

            foreach (ComponentAttachInfo info in infos)
            {
                switch (info.ReturnValue)
                {
                    case 0:
                        exists.Add(info);
                        break;
                    case 1:
                        success.Add(info);
                        break;
                    case -1:
                        error.Add(info);
                        break;
                    case 2:
                        successButSoMuch.Add(info);
                        break;
                    case -2:
                        sceneNotExists.Add(info);
                        break;
                    case 3:
                        successForArray.Add(info);
                        break;
                    case -3:
                        noWay.Add(info);
                        break;
                    default:
                        throw new System.Exception("Неизвестный тип возвращаемого значения в ComponentAttachInfo: " + info.ReturnValue);
                }
            }

            Debug.Log($"Всего найдено {allComponents} компонентов");
            Debug.Log("------------------------------------------");
            ShowInfoByList("Уже заполнено", exists);
            ShowInfoByList("Успешно заполнены", success);
            ShowInfoByList("Произошла ошибка", error);
            ShowInfoByList("Заполнено, но возможно не то", successButSoMuch);
            ShowInfoByList("Нет на сцене", sceneNotExists);
            ShowInfoByList("Заполнены массивы", successForArray);
            ShowInfoByList("Сюда невозможно прийти", noWay);

            Debug.Log("ИТОГО:");
            Debug.Log("------------------------------------------");
            ShowResults("Уже заполнено", exists);
            ShowResults("Успешно заполнены", success);
            ShowResults("Произошла ошибка", error);
            ShowResults("Заполнено, но возможно не то", successButSoMuch);
            ShowResults("Нет на сцене", sceneNotExists);
            ShowResults("Заполнены массивы", successForArray);
            ShowResults("Сюда невозможно прийти", noWay);            //Debug.Log($"Удалось найти {gameComponents.Length} gameObject-ов. Из них автоматически заполнились: {allComponents}");
        }

        private void ShowResults(string allMessage, List<ComponentAttachInfo> currentList)
        {
            Debug.Log($"{allMessage}: {currentList.Count}:");
        }

        private void ShowInfoByList(string allMessage, List<ComponentAttachInfo> currentList)
        {
            Debug.Log($"{allMessage}: {currentList.Count}:");

            foreach (ComponentAttachInfo info in currentList)
            {
                Debug.Log(info.ComponentInfo);
            }

            Debug.Log("------------------------------------------");
        }

        [ContextMenu(nameof(DefineAllComponents))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineEndTurnButton(),
                DefineCardRoot(),
                DefineGameFieldRoot(),
                DefineScreenRoot(),
                DefineFontRoot(),
                DefinePersonCreator(),
                DefineObjectsLightControlsCreator(),
                DefineForgingZone()
            };

            return list;
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private ComponentAttachInfo DefineEndTurnButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardRoot))]
        private ComponentAttachInfo DefineCardRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineGameFieldRoot))]
        private ComponentAttachInfo DefineGameFieldRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _gameFieldRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineScreenRoot))]
        private ComponentAttachInfo DefineScreenRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _screenRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFontRoot))]
        private ComponentAttachInfo DefineFontRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fontRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefinePersonCreator))]
        private ComponentAttachInfo DefinePersonCreator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _personCreator, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineObjectsLightControlsCreator))]
        private ComponentAttachInfo DefineObjectsLightControlsCreator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightControlsCreator, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineForgingZone))]
        private ComponentAttachInfo DefineForgingZone()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _forgingZone, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}