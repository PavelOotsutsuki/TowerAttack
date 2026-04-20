using System;
using System.Collections.Generic;
using Cards.Views.BigCardViews.Capabilities;
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
    public class StartMenuRoot : LocalRoot, IActivatable, IReactivatable
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

        public void Init(Action onPlayClick)
        {
            //CanvasScaler[] objectCanvasScalers = gameObject.GetComponentsInChildren<CanvasScaler>(true);
            //_canvasRoot.SetReferenceResolution(objectCanvasScalers);

            //TMP_Text[] objectTexts = gameObject.GetComponentsInChildren<TMP_Text>(true);
            //_fontRoot.SetFont(objectTexts);
            base.Init();
            //_fontRoot.Init();
            //_canvasRoot.Init();

            _startMenuLoadActions.Init(_backgroundSoundConfig, _foregroundSoundConfig, _cardCapabilityDescription,
                onPlayClick);
        }

        public override void Activate()
        {
            _startMenuLoadActions.Activate();
        }

        public void Reactivate()
        {
            _startMenuLoadActions.Reactivate();
        }

        public override void ActivateInputSystem()
        {
            _startMenuLoadActions.InputRoot.Activate();
        }

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