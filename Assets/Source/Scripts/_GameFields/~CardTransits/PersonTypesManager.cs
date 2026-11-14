using System;
using GameFields.Persons;
using Tools;

namespace GameFields.CardTransits
{
    public class PersonTypesManager : IData
    {
        private readonly PersonTypes _player;
        private readonly PersonTypes _enemy;

        public PersonTypesManager()
        {
            HandTypes playerHandTypes = new HandTypes(ViewType.HandPlayer, TransitToType.HandPlayer, TransitFromType.HandPlayer);
            _player = new PersonTypes(playerHandTypes, TransitToType.PlayerFirePool, ViewType.TablePlayer);

            HandTypes enemyHandTypes = new HandTypes(ViewType.HandAI, TransitToType.HandEnemy, TransitFromType.HandEnemy);
            _enemy = new PersonTypes(enemyHandTypes, TransitToType.EnemyFirePool, ViewType.TableAI);
        }

        public PersonTypes GetPersonTypes(IPersonObject owner)
        {
            switch (owner)
            {
                case IPlayerObject:
                    return _player;
                case IEnemyAIObject:
                    return _enemy;
                default:
                    throw new Exception($"Неизвестный {typeof(IPersonObject)}: {owner}; {owner.GetType()}");
            }
        }
    }
}