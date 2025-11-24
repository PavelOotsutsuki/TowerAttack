using System.Linq;
using Cards.Sounds;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class AwakeSoundReproducer
    {
        private readonly CardSoundRoot _cardSoundRoot;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly IPersonsState _personsState;

        public AwakeSoundReproducer(CardSoundRoot cardSoundRoot, CardLocationViewRoot viewRoot, IPersonsState personsState)
        {
            _cardSoundRoot = cardSoundRoot;
            _viewRoot = viewRoot;
            _personsState = personsState;
        }

        public void Play(CardSoundLogic cardSoundLogic)
        {
            switch (cardSoundLogic)
            {
                case MafiaBossCardSoundLogic mafiaBossCardSoundLogic:
                    MafiaBossAwakeSoundPlay(mafiaBossCardSoundLogic);
                    break;
                case IAwakeSoundKeeper awakeSoundKeeper:
                    DefaultAwakeSoundPlay(awakeSoundKeeper);
                    break;
            }
        }

        private void DefaultAwakeSoundPlay(IAwakeSoundKeeper awakeSoundKeeper)
        {
            _cardSoundRoot.Play(awakeSoundKeeper.AwakeSound);
        }

        private void MafiaBossAwakeSoundPlay(MafiaBossCardSoundLogic mafiaBossCardSoundLogic)
        {
            ViewType hand = _personsState.Active is Player ? ViewType.HandAI : ViewType.HandPlayer;
            int countCardsInHand = _viewRoot.GetAllCards(hand).Count();

            AudioClip awakeClip;

            if (countCardsInHand == 0)
            {
                awakeClip = mafiaBossCardSoundLogic.ZeroCardAwakeSound;
            }
            else if (countCardsInHand == 1)
            {
                awakeClip = mafiaBossCardSoundLogic.OneCardAwakeSound;
            }
            else
            {
                awakeClip = mafiaBossCardSoundLogic.DefaultAwakeSound;
            }

            _cardSoundRoot.Play(awakeClip);
        }
    }
}