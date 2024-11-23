using System.Collections;
using Cards;
using GameFields.Persons.AttackMenues;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class CardAttackZone : MonoBehaviour, IAttackable
    {
        private IActivatable _attackMenu;
        private ReadOnlyRectTransform _towerTransform;

        private Card _currentCard;

        public void Init(IActivatable attackMenu, IReadOnlyRectTransformable tower)
        {
            _attackMenu = attackMenu;
            _towerTransform = tower.ReadOnlyRectTransform;
        }

        public void Attack(Card card)
        {
            _currentCard = card;

            StartCoroutine(ActivatingAttack());
        }

        private IEnumerator ActivatingAttack()
        {
            Movement cardMovement = _currentCard.CardMovement;

            Vector2 firstCoordinates = FindFirstCoordinates();

            cardMovement.MoveSmoothly(firstCoordinates, _currentCard.ReadOnlyRectTransform.GetRotationVector(), 1f, _currentCard.ReadOnlyRectTransform.GetLocalScale());

            yield return new WaitForSeconds(2f);

            _attackMenu.Activate();
        }

        private Vector2 FindFirstCoordinates()
        {
            Vector2 towerRightDownAngle = _towerTransform.GetRightDownAnglePosition();

            Vector2 cardSize = _currentCard.ReadOnlyRectTransform.GetSizeDelta();

            return towerRightDownAngle; //+ cardSize;
        }
    }
}