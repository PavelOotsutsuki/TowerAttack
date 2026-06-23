using System;
using System.Collections.Generic;
using System.Threading;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using Sounds;
using StartMenues;
using TMPro;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Roots
{
    public class StartMenuRoot : LocalRoot, IActivatable, IReactivatable<StartMenuButtonsPanelRootReactivateData>
    {
        //[SerializeField] private CanvasRoot _canvasRoot;
        //[SerializeField] private FontRoot _fontRoot;
        [SerializeField] private StartMenuLoadActions _startMenuLoadActions;
        //[SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;
        //[SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;
        //[SerializeField] private StartMenuSavedData _startMenuSavedData;
        //[SerializeField] private Creator _gameRootCreator;
        private BackgroundSoundConfig _backgroundSoundConfig;
        private ForegroundSoundConfig _foregroundSoundConfig;
        private CardCapabilityDescription _cardCapabilityDescription;

        [Inject]
        private void Construct(BackgroundSoundConfig backgroundSoundConfig, ForegroundSoundConfig foregroundSoundConfig,
            CardCapabilityDescription cardCapabilityDescription)
        {
            _backgroundSoundConfig = backgroundSoundConfig;
            _foregroundSoundConfig = foregroundSoundConfig;
            _cardCapabilityDescription = cardCapabilityDescription;

            //StartCoroutine(Initing(bus, deck, seatPool, cardDescription, handPlayer, informationLabel, lookCardMenu, variantCardCreator, soundRoot,
            //    cardSoundVolume, fightMenuActivateButton, screenRoot));
            //_canvasRoot.Init();
            //_fontRoot.Init();

            //CardCapabilityDescription cardCapabilityDescription = new CardCapabilityDescription();
            //_startMenuLoadActions.Init(_backgroundSoundConfig, _foregroundSoundConfig, cardCapabilityDescription,
            //    _gameRootCreator);

            //_startMenuLoadActions.Activate();
        }

        public void Init(Action<int> onPlayClick, CancellationToken gameRootToken)
        {
            //CanvasScaler[] objectCanvasScalers = gameObject.GetComponentsInChildren<CanvasScaler>(true);
            //_canvasRoot.SetReferenceResolution(objectCanvasScalers);

            //TMP_Text[] objectTexts = gameObject.GetComponentsInChildren<TMP_Text>(true);
            //_fontRoot.SetFont(objectTexts);
            base.Init(gameRootToken);
            //_fontRoot.Init();
            //_canvasRoot.Init();

            _startMenuLoadActions.Init(_backgroundSoundConfig, _foregroundSoundConfig, _cardCapabilityDescription,
                onPlayClick, this.Token);
        }

        public override void Activate()
        {
            _startMenuLoadActions.Activate();

            //Test().Forget();
        }

        public void Reactivate(StartMenuButtonsPanelRootReactivateData reactivateDataInvoker)
        {
            _startMenuLoadActions.Reactivate(reactivateDataInvoker);
        }

        public override void ActivateInputSystem()
        {
            _startMenuLoadActions.InputRoot.Activate();
        }

        //private async UniTask Test()
        //{
        //    int s = 0;

        //    while (s < 60)
        //    {
        //        Debug.Log("seconds: " + s);
        //        await UniTask.WaitForSeconds(1f);

        //        s++;
        //    }

        //    Destroy(gameObject);
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(StartMenuRoot))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
  
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }
        #endregion
    }
}