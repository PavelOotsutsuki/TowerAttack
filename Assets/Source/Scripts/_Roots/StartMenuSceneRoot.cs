using System.Collections.Generic;
using Sounds;
using StartMenues;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class StartMenuSceneRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private CanvasRoot _canvasRoot;
        [SerializeField] private FontRoot _fontRoot;
        [SerializeField] private StartMenuLoadActions _startMenuLoadActions;
        [SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;
        [SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;

        private void Start()
        {
            //StartCoroutine(Initing(bus, deck, seatPool, cardDescription, handPlayer, informationLabel, lookCardMenu, variantCardCreator, soundRoot,
            //    cardSoundVolume, fightMenuActivateButton, screenRoot));
            _canvasRoot.Init();
            _fontRoot.Init();
            _startMenuLoadActions.Init(_backgroundSoundConfig, _foregroundSoundConfig);

            _startMenuLoadActions.Activate();
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
                DefineCanvasRoot(),
                DefineFontRoot()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasRoot))]
        private ComponentAttachInfo DefineCanvasRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFontRoot))]
        private ComponentAttachInfo DefineFontRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fontRoot, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}