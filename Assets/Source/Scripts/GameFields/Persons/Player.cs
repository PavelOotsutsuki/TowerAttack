using GameFields.Persons.Discovers;
using GameFields.DiscardPiles;
using UnityEngine;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;
using System.Collections;

namespace GameFields.Persons
{
    //[Serializable]
    internal class Player : Person
    {
        //[SerializeField] private Discover _discover;

        private Discover _discover;
        private ITableActivator _tableActivator;
        private IBlockable _handBlockable;
        private readonly MonoBehaviour _coroutineContainer;

        public Player(DiscardPile discardPile, ITableActivator tableActivator, HandPlayer hand, Table table, Tower tower,
            Discover discover, DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, MonoBehaviour coroutineContainer)
            : base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw)
        {
            _discover = discover;
            _tableActivator = tableActivator;
            _handBlockable = hand;
            _coroutineContainer = coroutineContainer;
        }

        public override void Init()
        {
            _discover.Deactivate();
        }

        public override void StartTurn()
        {
            _handBlockable.Block();
            _tableActivator.Activate();

            _coroutineContainer.StartCoroutine(ProcessingTurn());


            //DrawCardRoot.StartTurnDraw(_handBlockable.Unblock);
        }

        private IEnumerator ProcessingTurn()
        {
            //StartTurnDraw.PrepareToStart();
            StartTurnDraw.StartStep();

            yield return new WaitUntil(()=> StartTurnDraw.IsComplete);

            _handBlockable.Unblock();
        }
    }
}
