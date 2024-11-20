using Cards;
using GameFields.Persons.AttackMenues;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class CardAttackZone : MonoBehaviour, IAttackable
    {
        private IActivatable _attackMenu;

        public void Init(IActivatable attackMenu)
        {
            _attackMenu = attackMenu;
        }

        public void Attack()
        {
            _attackMenu.Activate();
        }
    }
}