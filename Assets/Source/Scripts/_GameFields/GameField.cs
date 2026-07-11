using UnityEngine;
using GameFields.StartFights;
using GameFields.Effects;
using GameFields.Persons;
using Tools.Utils.FillComponents;
using System.Collections.Generic;
using Zenject;
using GameFields.EndFights;
using GameFields.Seats;
using Tools;
using System;
using System.Threading;
using Servers;

namespace GameFields
{
    public class GameField : MonoBehaviour, IActivatable, IAutomaticFillComponents
    {
        [SerializeField] private StartFight _startFight;
        [SerializeField] private EndFight _endFight;
        //[SerializeField] private AudioClip _backgroundMusic;
        [Inject] private DBRoot _dBRoot;

        private EffectFactory _effectFactory;
        private FightStepsController _fightStepsController;

        public void Init(PersonsState personsState, EnemyAI enemyAI, SignalBus bus, SeatPool seatPool,
            IActivatable soundRootActivatable, IActivatable fightButtonsActivator, Action onDestroyPrefab,
            CancellationToken gameFieldToken, CancellationToken fightToken, CancellationTokenSource fightCTS,
            TurnToken turnToken)
        {
            _startFight.Init(enemyAI, gameFieldToken);

            FightResult fightResult = new FightResult();
            Fight fight = new Fight(personsState, fightResult, bus, seatPool
                , soundRootActivatable, fightButtonsActivator, fightToken, turnToken, _dBRoot);
            _endFight.Init(fightResult, onDestroyPrefab, gameFieldToken, fightCTS);
            _fightStepsController = new FightStepsController(_startFight, fight, _endFight, gameFieldToken);

            //_fightStepsController.NextStep();
            //_fightStepsController.StartStep();
        }

        public void Activate()
        {
            _fightStepsController.StartStep();
        }


        //private void Update()
        //{
        //    _fightStepsController.Update();
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(GameField))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartFight(),
                DefineEndFight()
            };

            return list;
        }

        [ContextMenu(nameof(DefineStartFight))]
        private ComponentAttachInfo DefineStartFight()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _startFight, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndFight))]
        private ComponentAttachInfo DefineEndFight()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _endFight, ComponentLocationTypes.InScene);
        }
        #endregion 
    }
}