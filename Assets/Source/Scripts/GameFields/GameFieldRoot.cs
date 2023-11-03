using Cards;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class GameFieldRoot : MonoBehaviour
    {
        [SerializeField] private GameFieldPlayer _gameFieldPlayer;
        //[SerializeField] private GameFieldEnemy _gameFieldEnemy;

        public void Init(Card[] cards)
        {
            InitGameFieldPlayer(cards);
        }

        private void InitGameFieldPlayer(Card[] cards)
        {
            _gameFieldPlayer.Init(cards);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineGameFieldPlayer();
        }

        [ContextMenu(nameof(DefineGameFieldPlayer))]
        private void DefineGameFieldPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _gameFieldPlayer, ComponentLocationTypes.InChildren);
        }
    }
}
