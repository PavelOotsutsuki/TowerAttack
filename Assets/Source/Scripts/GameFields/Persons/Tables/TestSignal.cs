using System;
using System.ComponentModel;
using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public struct TestSignal
    {
        public TestSignal(Card card, ICardDropPlace dropPlace)
        {
            Card = card;
            DropPlace = dropPlace;
        }

        public Card Card { get; private set; }
        public ICardDropPlace DropPlace { get; private set; }
    }

    public class SignalTest : MonoBehaviour
    {
        private SignalBus _bus;

        private void OnEnable()
        {
            _bus.Subscribe<TestSignal>(OnSignal);
        }

        private void OnDisable()
        {
            _bus.Unsubscribe<TestSignal>(OnSignal);
        }

        private void OnSignal(TestSignal signal)
        {
            //сделать что-то
        }
        
        //-----------------------------В других классах------------------------------------
        //инсталлер
        private void InstallBindings()
        {
            //проинитить в истналлере
            Container.Declair<TestSignal>();
        }

        //в классе, который вызывает сигнал(событие)
        private void SignalCall()
        {
            Card card;
            ICardDropPlace dropPlace;
            _bus.Fire(new TestSignal(card, dropPlace));
        }
    }
    
}