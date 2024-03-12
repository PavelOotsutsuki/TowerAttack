using GameFields.DiscardPiles;
using GameFields.Persons;
using Tools;
using UnityEngine;
using Cards;

namespace GameFields.Effects
{
    public class EffectRoot
    {
        private Deck _deck;
        private DiscardPile _discardPile;
        private IPersonSideListener _personSideListener;

        public EffectRoot(Deck deck, DiscardPile discardPile, IPersonSideListener personSideListener)
        {
            _deck = deck;
            _discardPile = discardPile;
            _personSideListener = personSideListener;
        }

        public void PlayEffect(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.ZhyzhaEffect:
                    ZhyzhaEffect zhyzhaEffect = new ZhyzhaEffect(_personSideListener.DeactivePerson);
                    break;
                case EffectType.GreedyEffect:
                    GreedyEffect greedyEffect = new GreedyEffect(_personSideListener.ActivePerson, _personSideListener.DeactivePerson);
                    break;
                case EffectType.PatriarchCorallEffect:
                    PatriarchCorallEffect patriarchCorallEffect = new PatriarchCorallEffect(_deck, _personSideListener.ActivePerson, _personSideListener.DeactivePerson);
                    break;
                default:
                    Debug.Log("Effect is not founded");
                    break;
            }
        }
    }
}
