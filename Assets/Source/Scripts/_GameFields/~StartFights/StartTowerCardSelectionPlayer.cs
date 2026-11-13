using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.Decks;
using GameFields.Persons.Discovers;
using GameFields.Seats;
using UnityEngine;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using Cards.Views;
using GameFields.CardTransits;
using System.Threading;
using System;
using Servers;
using GameFields.Persons;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayer : StartTowerCardSelection, IPlayerObject
    {
        private readonly Deck _deck;
        private readonly ICardSeatable _hand;
        private readonly Discover _discover;
        private readonly FightProcessDBManager _fightProcessDBManager;

        private readonly StartTowerCardSelectionPlayerData _data;
        private readonly InvertCardAnimation _invertCardAnimation;

        private readonly Seat[] _seats;

        public StartTowerCardSelectionPlayer(Deck deck, HandPlayer hand, TowerPlayer tower, Seat[] seats, Discover discover, StartTowerCardSelectionPlayerData data,
            FightProcessDBManager fightProcessDBManager) : base(tower)
        {
            _hand = hand;
            _data = data;
            _fightProcessDBManager = fightProcessDBManager;

            _seats = seats;
            _deck = deck;
            _discover = discover;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);

            InitSeats();
        }

        public override void StartProcess(CancellationToken token)
        {
            StartPlayerProcess(token).Forget();
        }

        private async UniTask StartPlayerProcess(CancellationToken token)
        {
            try
            {
                List<Card> cards = new List<Card>();

                await UniTask.WaitForSeconds(_data.WaitDurationForEnemyFirstCardsDraw, cancellationToken: token);

                for (int i = 0; i < _seats.Length; i++)
                {
                    Card card = _deck.TakeTopCard();
                    cards.Add(card);
                    _seats[i].SetCard(card, SideType.Front, _data.DrawCardsDuration, _data.DrawCardsScaleFactor);

                    await UniTask.WaitForSeconds(_data.WaitDurationBetweenDrawCards, cancellationToken: token);
                }

                if (_data.DrawCardsDuration - _data.WaitDurationBetweenDrawCards > 0)
                {
                    await UniTask.WaitForSeconds(_data.DrawCardsDuration - _data.WaitDurationBetweenDrawCards, cancellationToken: token);
                }

                foreach (Card card in cards)
                {
                    card.gameObject.SetActive(false);
                }

                DiscoverResult discoverResult = new DiscoverResult(callbackAfterSetResult: ActivateSeats);
                DiscoverActivateData discoverActivateData = new DiscoverActivateData(cards, _data.LabelMessage, discoverResult);
                _discover.Activate(discoverActivateData);

                await UniTask.WaitUntil(() => discoverResult.Result != null, cancellationToken: token);

                //foreach (Seat seat in _seats)
                //{
                //    seat.Card.gameObject.SetActive(true);
                //}

                //EndProcessing(discoverResult.Result).ToUniTask();
                await UniTask.WaitForSeconds(_data.DelayAfterCardChoiceDone, cancellationToken: token);

                if (Tower.HasFreeSeat)
                {
                    foreach (Seat seat in _seats)
                    {
                        if (ReferenceEquals(seat.Card, discoverResult.Result) == false)
                        {
                            _hand.SeatCard(seat.Card);
                            //seat.Card.SetActiveInteraction(true);
                            //seat.Reset();
                        }
                        else
                        {
                            SeatCardInTower(seat, token).Forget();
                        }
                    }

                    // Ещё раз потому что при SeatCard идет перерасчет и interactable сбрасывается
                    foreach (Seat seat in _seats)
                    {
                        if (ReferenceEquals(seat.Card, discoverResult.Result) == false)
                        {
                            seat.Card.SetActiveInteraction(false);
                            seat.Reset();
                        }
                    }
                }
                else
                {
                    throw new System.Exception("Что-то не так, работяги");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        private void ActivateSeats(IDiscoverable card)
        {
            foreach (Seat seat in _seats)
            {
                seat.Card.gameObject.SetActive(true);
            }
        }

        //private void OnCardChoiceDone(Card card)
        //{
        //    foreach (Seat seat in _seats)
        //    {
        //        seat.Card.gameObject.SetActive(true);
        //    }

        //    EndProcessing(card).ToUniTask();
        //}

        //private IEnumerator EndProcessing(Card card)
        //{
        //    yield return new WaitForSeconds(_data.DelayAfterCardChoiceDone);

        //    if (TowerTransitCheck.IsFill == false)
        //    {
        //        foreach (Seat seat in _seats)
        //        {
        //            if (seat.Card != card)
        //            {
        //                _handTransitSet.Set(seat.Card);
        //                seat.Card.SetActiveInteraction(false);
        //                seat.Reset();
        //            }
        //            else
        //            {
        //                SeatCardInTower(seat).ToUniTask();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new System.Exception("Что-то не так, работяги");
        //    }
        //}

        private async UniTask SeatCardInTower(Seat mySeat, CancellationToken token)
        {
            Card card = mySeat.Card;

            await UniTask.WaitForSeconds(_data.DelayBeforeStartProcessSeatCardInTower, cancellationToken: token);

            _invertCardAnimation.Play(card, token);

            await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: token);

            //InvertCardFront(card);
            //yield return new WaitForSeconds(_data.InvertCardFrontDuration);

            //card.SetSide(SideType.Back);

            //InvertCardBack(card);
            //yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            mySeat.Reset();
            Tower.SeatCard(card);
        }

        //private void InvertCardFront(Card card)
        //{
        //    Vector3 position = card.ReadOnlyRectTransform.GetPosition();

        //    Movement cardMovement = card.CardMovement;

        //    cardMovement.MoveLinear(position, new Vector3(0f, -90f, 0f), _data.InvertCardFrontDuration);
        //}

        //private void InvertCardBack(Card card)
        //{
        //    Vector3 endRotationVector = Vector3.zero;
        //    Vector3 position = card.ReadOnlyRectTransform.GetPosition();

        //    Movement cardMovement = card.CardMovement;

        //    cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, card.ReadOnlyRectTransform.GetLocalScale());
        //}

        private void InitSeats()
        {
            foreach (Seat seat in _seats)
            {
                seat.Init(_fightProcessDBManager);
                seat.SetOwner(nameof(StartTowerCardSelectionPlayer), true);
            }
        }
    }
}